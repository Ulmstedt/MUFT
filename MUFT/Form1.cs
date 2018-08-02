using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace MUFT
{
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
            FileTransferConnection connection;

            string ip = IPTextBox.Text;
            int port;
            if(!Int32.TryParse(portTextBox.Text, out port))
            {
                MessageBox.Show("Invalid port");
                return;
            }

            // Setup connection
            if (radioClient.Checked)
            {
                connection = new Client(ip, port);
            }
            else
            {
                connection = new Server(port);
            }

            connection.Connect();
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
                    SimpleFileInfo file = new SimpleFileInfo(path, fi.Length, 0);
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
    }
}
