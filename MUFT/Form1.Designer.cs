namespace MUFT
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.IPLabel = new System.Windows.Forms.Label();
            this.IPTextBox = new System.Windows.Forms.TextBox();
            this.portLabel = new System.Windows.Forms.Label();
            this.portTextBox = new System.Windows.Forms.TextBox();
            this.radioClient = new System.Windows.Forms.RadioButton();
            this.radioServer = new System.Windows.Forms.RadioButton();
            this.clientServerPanel = new System.Windows.Forms.Panel();
            this.networkGroup = new System.Windows.Forms.GroupBox();
            this.connectButton = new System.Windows.Forms.Button();
            this.filesGroup = new System.Windows.Forms.GroupBox();
            this.fileListView = new System.Windows.Forms.ListView();
            this.statusHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pathHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.sizeHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.totalProgress = new System.Windows.Forms.ProgressBar();
            this.currentFileGroup = new System.Windows.Forms.GroupBox();
            this.totalFilesGroup = new System.Windows.Forms.GroupBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.sendReceivePanel = new System.Windows.Forms.Panel();
            this.receiveRadio = new System.Windows.Forms.RadioButton();
            this.sendRadio = new System.Windows.Forms.RadioButton();
            this.clientServerPanel.SuspendLayout();
            this.networkGroup.SuspendLayout();
            this.filesGroup.SuspendLayout();
            this.currentFileGroup.SuspendLayout();
            this.totalFilesGroup.SuspendLayout();
            this.sendReceivePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // IPLabel
            // 
            this.IPLabel.AutoSize = true;
            this.IPLabel.Location = new System.Drawing.Point(8, 18);
            this.IPLabel.Name = "IPLabel";
            this.IPLabel.Size = new System.Drawing.Size(20, 13);
            this.IPLabel.TabIndex = 0;
            this.IPLabel.Text = "IP:";
            // 
            // IPTextBox
            // 
            this.IPTextBox.Location = new System.Drawing.Point(34, 15);
            this.IPTextBox.Name = "IPTextBox";
            this.IPTextBox.Size = new System.Drawing.Size(106, 20);
            this.IPTextBox.TabIndex = 1;
            // 
            // portLabel
            // 
            this.portLabel.AutoSize = true;
            this.portLabel.Location = new System.Drawing.Point(146, 18);
            this.portLabel.Name = "portLabel";
            this.portLabel.Size = new System.Drawing.Size(29, 13);
            this.portLabel.TabIndex = 2;
            this.portLabel.Text = "Port:";
            // 
            // portTextBox
            // 
            this.portTextBox.Location = new System.Drawing.Point(181, 15);
            this.portTextBox.Name = "portTextBox";
            this.portTextBox.Size = new System.Drawing.Size(41, 20);
            this.portTextBox.TabIndex = 3;
            // 
            // radioClient
            // 
            this.radioClient.AutoSize = true;
            this.radioClient.Checked = true;
            this.radioClient.Location = new System.Drawing.Point(3, 3);
            this.radioClient.Name = "radioClient";
            this.radioClient.Size = new System.Drawing.Size(51, 17);
            this.radioClient.TabIndex = 4;
            this.radioClient.TabStop = true;
            this.radioClient.Text = "Client";
            this.radioClient.UseVisualStyleBackColor = true;
            this.radioClient.CheckedChanged += new System.EventHandler(this.radioClient_CheckedChanged);
            // 
            // radioServer
            // 
            this.radioServer.AutoSize = true;
            this.radioServer.Location = new System.Drawing.Point(60, 3);
            this.radioServer.Name = "radioServer";
            this.radioServer.Size = new System.Drawing.Size(56, 17);
            this.radioServer.TabIndex = 5;
            this.radioServer.Text = "Server";
            this.radioServer.UseVisualStyleBackColor = true;
            this.radioServer.CheckedChanged += new System.EventHandler(this.radioServer_CheckedChanged);
            // 
            // clientServerPanel
            // 
            this.clientServerPanel.Controls.Add(this.radioServer);
            this.clientServerPanel.Controls.Add(this.radioClient);
            this.clientServerPanel.Location = new System.Drawing.Point(228, 15);
            this.clientServerPanel.Name = "clientServerPanel";
            this.clientServerPanel.Size = new System.Drawing.Size(118, 23);
            this.clientServerPanel.TabIndex = 6;
            // 
            // networkGroup
            // 
            this.networkGroup.Controls.Add(this.sendReceivePanel);
            this.networkGroup.Controls.Add(this.connectButton);
            this.networkGroup.Controls.Add(this.IPTextBox);
            this.networkGroup.Controls.Add(this.clientServerPanel);
            this.networkGroup.Controls.Add(this.IPLabel);
            this.networkGroup.Controls.Add(this.portTextBox);
            this.networkGroup.Controls.Add(this.portLabel);
            this.networkGroup.Location = new System.Drawing.Point(1, 346);
            this.networkGroup.Name = "networkGroup";
            this.networkGroup.Size = new System.Drawing.Size(562, 43);
            this.networkGroup.TabIndex = 7;
            this.networkGroup.TabStop = false;
            this.networkGroup.Text = "Network";
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(483, 15);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(75, 23);
            this.connectButton.TabIndex = 7;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // filesGroup
            // 
            this.filesGroup.Controls.Add(this.fileListView);
            this.filesGroup.Location = new System.Drawing.Point(1, 97);
            this.filesGroup.Name = "filesGroup";
            this.filesGroup.Size = new System.Drawing.Size(562, 243);
            this.filesGroup.TabIndex = 8;
            this.filesGroup.TabStop = false;
            this.filesGroup.Text = "Files";
            // 
            // fileListView
            // 
            this.fileListView.AllowDrop = true;
            this.fileListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.statusHeader,
            this.pathHeader,
            this.sizeHeader});
            this.fileListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fileListView.FullRowSelect = true;
            this.fileListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.fileListView.LabelEdit = true;
            this.fileListView.Location = new System.Drawing.Point(3, 16);
            this.fileListView.Name = "fileListView";
            this.fileListView.Size = new System.Drawing.Size(556, 224);
            this.fileListView.TabIndex = 0;
            this.fileListView.UseCompatibleStateImageBehavior = false;
            this.fileListView.View = System.Windows.Forms.View.Details;
            this.fileListView.SelectedIndexChanged += new System.EventHandler(this.fileList_SelectedIndexChanged);
            // 
            // statusHeader
            // 
            this.statusHeader.Text = "S";
            this.statusHeader.Width = 25;
            // 
            // pathHeader
            // 
            this.pathHeader.Text = "File";
            this.pathHeader.Width = 456;
            // 
            // sizeHeader
            // 
            this.sizeHeader.Text = "Size";
            this.sizeHeader.Width = 75;
            // 
            // totalProgress
            // 
            this.totalProgress.Enabled = false;
            this.totalProgress.Location = new System.Drawing.Point(4, 17);
            this.totalProgress.Name = "totalProgress";
            this.totalProgress.Size = new System.Drawing.Size(555, 23);
            this.totalProgress.Step = 1;
            this.totalProgress.TabIndex = 9;
            // 
            // currentFileGroup
            // 
            this.currentFileGroup.Controls.Add(this.totalProgress);
            this.currentFileGroup.Location = new System.Drawing.Point(1, 3);
            this.currentFileGroup.Margin = new System.Windows.Forms.Padding(5);
            this.currentFileGroup.Name = "currentFileGroup";
            this.currentFileGroup.Size = new System.Drawing.Size(562, 48);
            this.currentFileGroup.TabIndex = 10;
            this.currentFileGroup.TabStop = false;
            this.currentFileGroup.Text = "Current";
            // 
            // totalFilesGroup
            // 
            this.totalFilesGroup.Controls.Add(this.progressBar1);
            this.totalFilesGroup.Location = new System.Drawing.Point(1, 51);
            this.totalFilesGroup.Margin = new System.Windows.Forms.Padding(5);
            this.totalFilesGroup.Name = "totalFilesGroup";
            this.totalFilesGroup.Size = new System.Drawing.Size(562, 48);
            this.totalFilesGroup.TabIndex = 11;
            this.totalFilesGroup.TabStop = false;
            this.totalFilesGroup.Text = "Total";
            // 
            // progressBar1
            // 
            this.progressBar1.Enabled = false;
            this.progressBar1.Location = new System.Drawing.Point(4, 17);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(555, 23);
            this.progressBar1.Step = 1;
            this.progressBar1.TabIndex = 9;
            // 
            // sendReceivePanel
            // 
            this.sendReceivePanel.Controls.Add(this.receiveRadio);
            this.sendReceivePanel.Controls.Add(this.sendRadio);
            this.sendReceivePanel.Location = new System.Drawing.Point(352, 15);
            this.sendReceivePanel.Name = "sendReceivePanel";
            this.sendReceivePanel.Size = new System.Drawing.Size(125, 23);
            this.sendReceivePanel.TabIndex = 7;
            // 
            // receiveRadio
            // 
            this.receiveRadio.AutoSize = true;
            this.receiveRadio.Location = new System.Drawing.Point(60, 3);
            this.receiveRadio.Name = "receiveRadio";
            this.receiveRadio.Size = new System.Drawing.Size(65, 17);
            this.receiveRadio.TabIndex = 5;
            this.receiveRadio.Text = "Receive";
            this.receiveRadio.UseVisualStyleBackColor = true;
            // 
            // sendRadio
            // 
            this.sendRadio.AutoSize = true;
            this.sendRadio.Checked = true;
            this.sendRadio.Location = new System.Drawing.Point(3, 3);
            this.sendRadio.Name = "sendRadio";
            this.sendRadio.Size = new System.Drawing.Size(50, 17);
            this.sendRadio.TabIndex = 4;
            this.sendRadio.TabStop = true;
            this.sendRadio.Text = "Send";
            this.sendRadio.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(564, 389);
            this.Controls.Add(this.totalFilesGroup);
            this.Controls.Add(this.currentFileGroup);
            this.Controls.Add(this.filesGroup);
            this.Controls.Add(this.networkGroup);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MUFT";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.clientServerPanel.ResumeLayout(false);
            this.clientServerPanel.PerformLayout();
            this.networkGroup.ResumeLayout(false);
            this.networkGroup.PerformLayout();
            this.filesGroup.ResumeLayout(false);
            this.currentFileGroup.ResumeLayout(false);
            this.totalFilesGroup.ResumeLayout(false);
            this.sendReceivePanel.ResumeLayout(false);
            this.sendReceivePanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label IPLabel;
        private System.Windows.Forms.TextBox IPTextBox;
        private System.Windows.Forms.Label portLabel;
        private System.Windows.Forms.TextBox portTextBox;
        private System.Windows.Forms.RadioButton radioClient;
        private System.Windows.Forms.RadioButton radioServer;
        private System.Windows.Forms.Panel clientServerPanel;
        private System.Windows.Forms.GroupBox networkGroup;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.GroupBox filesGroup;
        private System.Windows.Forms.ListView fileListView;
        private System.Windows.Forms.ColumnHeader pathHeader;
        private System.Windows.Forms.ProgressBar totalProgress;
        private System.Windows.Forms.GroupBox currentFileGroup;
        private System.Windows.Forms.GroupBox totalFilesGroup;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.ColumnHeader statusHeader;
        private System.Windows.Forms.ColumnHeader sizeHeader;
        private System.Windows.Forms.Panel sendReceivePanel;
        private System.Windows.Forms.RadioButton receiveRadio;
        private System.Windows.Forms.RadioButton sendRadio;
    }
}