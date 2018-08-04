using System;
using System.Collections.Generic;
using System.ComponentModel;
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

    // Arguments needed to update the progress bars
    struct ProgressArgs
    {
        public int currentProgress;
        public int totalProgress;
        public ProgressArgs(int curr, int total)
        {
            this.currentProgress = curr;
            this.totalProgress = total;
        }
    }

    public partial class MainForm : Form
    {
        private List<SimpleFileInfo> fileList;
        private int numFiles = 0;
        private long totalSize = 0;

        

        public MainForm()
        {
            fileList = new List<SimpleFileInfo>();
            InitializeComponent();
        }

        private void UpdateTotalSize()
        {
            totalFilesGroup.Text = "Total (0 / " + SimpleFileInfo.SizeToString(totalSize) + ")";
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
                // Allow the user to select where to save the received files
                folderBrowser.ShowDialog();
                args.path = folderBrowser.SelectedPath;
            }

            BackgroundWorker bgw = new BackgroundWorker();
            bgw.WorkerReportsProgress = true;
            bgw.ProgressChanged += bgw_ProgressChanged;
            bgw.DoWork += bgw_TransferFiles;
            bgw.RunWorkerCompleted += bgw_TransferComplete;
            bgw.RunWorkerAsync(args);
        }

        void bgw_TransferFiles(object sender, DoWorkEventArgs e)
        {
            fileListView.Enabled = false;

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

                connection.Connect();
                connection.SendFiles((BackgroundWorker)sender);
            }
            else if (radioReceive.Checked)
            {
                connection.Connect();
                connection.ReceiveFiles(args.path, currentProgress, totalProgress);
            }
        }

        void bgw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressArgs args = (ProgressArgs)e.UserState;
            Console.WriteLine(args.currentProgress);
            currentProgress.Value = args.currentProgress;
            currentProgress.Refresh();
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
            fileListView.Enabled = true;
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

                    // Create list view item
                    ListViewItem item = new ListViewItem();
                    item.Text = "-"; // Status
                    item.SubItems.Add(file.Path);
                    item.SubItems.Add(file.SizeString);
                    fileListView.Items.Add(item);
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

        private void folderBrowser_HelpRequest(object sender, EventArgs e)
        {

        }

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
