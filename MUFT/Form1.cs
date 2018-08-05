using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace MUFT
{
    // Arguments needed to start the file transfer with a background worker
    struct BgwArgs
    {
        public string ip;
        public int port;
        public string path;
        public BgwArgs(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
            this.path = "";
        }
    }

    // Arguments needed to update the progress
    struct ProgressArgs
    {
        public int currentProgress;
        public int totalProgress;
        public long currentSpeed;
        public long bytesTransfered;
        public long bytesTotal;
        public TimeSpan timeRemaining; // Seconds
        public SimpleFileInfo fileInfo;
        public ProgressArgs(SimpleFileInfo fileInfo, int currProg, int totalProg, long bytesTransfered, long bytesTotal, long currSpeed, TimeSpan timeRemaining)
        {
            this.fileInfo = fileInfo;
            this.currentProgress = currProg;
            this.totalProgress = totalProg;
            this.bytesTransfered = bytesTransfered;
            this.bytesTotal = bytesTotal;
            this.currentSpeed = currSpeed;
            this.timeRemaining = timeRemaining;
        }
    }

    public partial class MainForm : Form
    {
        private List<SimpleFileInfo> fileList;
        private int numFiles = 0;
        private long totalSize = 0;

        ProgressBarWithText totalProgress;

        public MainForm()
        {
            fileList = new List<SimpleFileInfo>();
            InitializeComponent();
        }

        private void UpdateTotalSize()
        {
            totalFilesGroup.Text = "Total (0 / " + Utilities.SizeToString(totalSize) + ")";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Drag and drop files into list view
            fileListView.DragEnter += fileListView_DragEnter;
            fileListView.DragDrop += fileListView_DragDrop;

            // Delete file entries with del
            fileListView.KeyDown += fileListView_KeyDown;

            //fileListView.ColumnWidthChanging += fileListView_ColumnWidthChanging;

            // ToolTips
            ToolTip filesToolTip = new ToolTip();
            filesToolTip.ShowAlways = true;
            filesToolTip.SetToolTip(fileListView, "Drag-and-drop to add files.");

            ToolTip IPToolTip = new ToolTip();
            IPToolTip.ShowAlways = true;
            IPToolTip.SetToolTip(IPTextBox, "IP Address of the server.");

            ToolTip portToolTip = new ToolTip();
            portToolTip.ShowAlways = true;
            portToolTip.SetToolTip(portTextBox, "Port of the server.");

            // Reduce width of listView in order to eliminate horizontal scroll bar
            //fileListView.Columns[1].Width -= (4 + SystemInformation.VerticalScrollBarWidth);

            totalProgress = new ProgressBarWithText();
            totalProgress.Enabled = false;
            totalProgress.Location = new Point(4, 17);
            totalProgress.Name = "currentProgress";
            totalProgress.Size = new Size(555, 23);
            totalProgress.Step = 1;
            totalProgress.TabIndex = 9;
            
            totalFilesGroup.Controls.Add(totalProgress);

        }


        private void radioServer_CheckedChanged(object sender, EventArgs e)
        {
            IPTextBox.Enabled = false;
        }

        private void radioClient_CheckedChanged(object sender, EventArgs e)
        {
            IPTextBox.Enabled = true;
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            string ip = IPTextBox.Text;
            int port;
            if (!Int32.TryParse(portTextBox.Text, out port))
            {
                MessageBox.Show("Invalid port");
                return;
            }

            BgwArgs args = new BgwArgs(ip, port);

            if (radioReceive.Checked)
            {
                fileListView.Clear();
                // Allow the user to select where to save the received files
                folderBrowser.ShowDialog();
                args.path = folderBrowser.SelectedPath;
                if(args.path == "")
                {
                    MessageBox.Show("Invalid save location");
                    return;
                }
            }

            DisableForm();

            BackgroundWorker bgw = new BackgroundWorker();
            bgw.WorkerReportsProgress = true;
            bgw.ProgressChanged += bgw_ProgressChanged;
            bgw.DoWork += bgw_TransferFiles;
            bgw.RunWorkerCompleted += bgw_TransferComplete;
            bgw.RunWorkerAsync(args);
        }

        void bgw_TransferFiles(object sender, DoWorkEventArgs e)
        {
            BgwArgs args = (BgwArgs)e.Argument;

            FileTransferConnection connection;

            // Setup connection
            if (radioClient.Checked)
            {
                connection = new Client(args.ip, args.port);
            }
            else
            {
                connection = new Server(args.port);
            }
            if (radioSend.Checked)
            {
                connection.FileList = fileList;
                connection.NumFiles = numFiles;
                connection.TotalSize = totalSize;

                connection.SendFiles((BackgroundWorker)sender);
            }
            else if (radioReceive.Checked)
            {
                connection.ReceiveFiles(args.path, (BackgroundWorker)sender);
            }
        }

        void bgw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressArgs args = (ProgressArgs)e.UserState;

            currentProgress.Value = args.currentProgress;
            totalProgress.Value = args.totalProgress;

            string progress = Utilities.SizeToString(args.bytesTransfered) + " / " + Utilities.SizeToString(args.bytesTotal);
            string speed = Utilities.SizeToString(args.currentSpeed) + "/s";
            string timeRemaining = Utilities.TimeToText(args.timeRemaining);
            totalProgress.CustomText = progress + " (" + speed + ") - " + timeRemaining + " remaining";

            if(args.fileInfo != null)
            {
                AddListViewItem("-", args.fileInfo.Path, args.fileInfo.SizeString);
            }

            currentProgress.Refresh();
            totalProgress.Refresh();
        }


        void bgw_TransferComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                MessageBox.Show("Transfer cancelled!");
            }
            else if (e.Error != null)
            {
                MessageBox.Show("Error: " + e.Error.Message);
            }
            else
            {
                MessageBox.Show("Transfer complete!");
            }

            currentProgress.Value = 0;
            totalProgress.Value = 0;
            totalProgress.CustomText = "";
            currentProgress.Refresh();
            totalProgress.Refresh();
            EnableForm();
        }

        void AddListViewItem(string status, string path, string size)
        {
            ListViewItem item = new ListViewItem();
            item.Text = status; // Status
            item.SubItems.Add(path);
            item.SubItems.Add(size);
            fileListView.Items.Add(item);
        }

        // Disables most components of the form (for when transfering files)
        void DisableForm()
        {
            fileListView.Enabled = false;
            IPTextBox.Enabled = false;
            portTextBox.Enabled = false;
            clientServerPanel.Enabled = false;
            sendReceivePanel.Enabled = false;
            connectButton.Enabled = false;
        }

        // Re-enables the form
        void EnableForm()
        {
            fileListView.Enabled = true;
            IPTextBox.Enabled = true;
            portTextBox.Enabled = true;
            clientServerPanel.Enabled = true;
            sendReceivePanel.Enabled = true;
            connectButton.Enabled = true;
        }

        #region File listView

        private void fileListView_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void fileListView_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] filePaths = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (string path in filePaths)
                {
                    // Get file info
                    FileInfo fi = new FileInfo(path);

                    // Create File and add to file list
                    SimpleFileInfo file = new SimpleFileInfo(fi.FullName, fi.Name, fi.Length, 0);
                    fileList.Add(file);
                    numFiles++;
                    totalSize += file.Size;

                    UpdateTotalSize();
                    AddListViewItem("-", file.Path, file.SizeString);
                }
            }
        }

        private void fileListView_KeyDown(object sender, KeyEventArgs e)
        {
            // Delete selected items on pressing del
            if (e.KeyCode == Keys.Delete)
            {
                foreach (ListViewItem item in ((ListView)sender).SelectedItems)
                {
                    numFiles--;
                    totalSize -= fileList[item.Index].Size;
                    UpdateTotalSize();

                    fileList.RemoveAt(item.Index);
                    item.Remove();
                }
            }
        }

        private void fileListView_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            // Dont allow columns to be resized
            e.Cancel = true;
            e.NewWidth = fileListView.Columns[e.ColumnIndex].Width;
        }

        private void fileList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        #endregion

        private void radioReceive_CheckedChanged(object sender, EventArgs e)
        {
            if (radioReceive.Checked)
            {
                fileListView.Enabled = false;
            }
            else
            {
                fileListView.Enabled = true;
            }
        }
    }
}
