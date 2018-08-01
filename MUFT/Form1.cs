using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MUFT
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Drag and drop files into list view
            fileList.DragEnter += fileList_DragEnter;
            fileList.DragDrop += fileList_DragDrop;

            // Delete file with del
            fileList.KeyDown += fileList_KeyDown;

            // ToolTips
            ToolTip filesToolTip = new ToolTip();
            filesToolTip.ShowAlways = true;
            filesToolTip.SetToolTip(fileList, "Drag-and-drop to add files.");

            ToolTip IPToolTip = new ToolTip();
            IPToolTip.ShowAlways = true;
            IPToolTip.SetToolTip(IPTextBox, "IP Address of the server.");

            ToolTip portToolTip = new ToolTip();
            portToolTip.ShowAlways = true;
            portToolTip.SetToolTip(portTextBox, "Port of the server.");
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
            ListViewItem item = new ListViewItem();
            item.Text = "X"; // Status
            item.SubItems.Add("Test 1");
            item.SubItems.Add("32 KB");
            fileList.Items.Add(item);
        }

        private void fileList_DragEnter(object sender, DragEventArgs e)
        {
            if(e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void fileList_DragDrop(object sender, DragEventArgs e)
        {
            if(e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] filePaths = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach(string path in filePaths)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = "X"; // Status
                    item.SubItems.Add(path);
                    item.SubItems.Add("32 KB");
                    fileList.Items.Add(item);
                }
            }
        }

        private void fileList_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Delete)
            {
                foreach(ListViewItem item in ((ListView)sender).SelectedItems)
                {
                    item.Remove();
                }
            }
        }

        private void fileList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
