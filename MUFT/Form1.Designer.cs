﻿namespace MUFT
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.networkGroup = new System.Windows.Forms.GroupBox();
            this.connectButton = new System.Windows.Forms.Button();
            this.filesGroup = new System.Windows.Forms.GroupBox();
            this.fileList = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel1.SuspendLayout();
            this.networkGroup.SuspendLayout();
            this.filesGroup.SuspendLayout();
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
            this.IPTextBox.Size = new System.Drawing.Size(110, 20);
            this.IPTextBox.TabIndex = 1;
            // 
            // portLabel
            // 
            this.portLabel.AutoSize = true;
            this.portLabel.Location = new System.Drawing.Point(151, 18);
            this.portLabel.Name = "portLabel";
            this.portLabel.Size = new System.Drawing.Size(29, 13);
            this.portLabel.TabIndex = 2;
            this.portLabel.Text = "Port:";
            // 
            // portTextBox
            // 
            this.portTextBox.Location = new System.Drawing.Point(186, 15);
            this.portTextBox.Name = "portTextBox";
            this.portTextBox.Size = new System.Drawing.Size(45, 20);
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
            // panel1
            // 
            this.panel1.Controls.Add(this.radioServer);
            this.panel1.Controls.Add(this.radioClient);
            this.panel1.Location = new System.Drawing.Point(237, 15);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(118, 23);
            this.panel1.TabIndex = 6;
            // 
            // networkGroup
            // 
            this.networkGroup.Controls.Add(this.connectButton);
            this.networkGroup.Controls.Add(this.IPTextBox);
            this.networkGroup.Controls.Add(this.panel1);
            this.networkGroup.Controls.Add(this.IPLabel);
            this.networkGroup.Controls.Add(this.portTextBox);
            this.networkGroup.Controls.Add(this.portLabel);
            this.networkGroup.Location = new System.Drawing.Point(0, 251);
            this.networkGroup.Name = "networkGroup";
            this.networkGroup.Size = new System.Drawing.Size(446, 43);
            this.networkGroup.TabIndex = 7;
            this.networkGroup.TabStop = false;
            this.networkGroup.Text = "Network";
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(361, 15);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(75, 23);
            this.connectButton.TabIndex = 7;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // filesGroup
            // 
            this.filesGroup.Controls.Add(this.fileList);
            this.filesGroup.Location = new System.Drawing.Point(0, 2);
            this.filesGroup.Name = "filesGroup";
            this.filesGroup.Size = new System.Drawing.Size(446, 243);
            this.filesGroup.TabIndex = 8;
            this.filesGroup.TabStop = false;
            this.filesGroup.Text = "Files";
            // 
            // fileList
            // 
            this.fileList.AllowDrop = true;
            this.fileList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.fileList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fileList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.fileList.LabelEdit = true;
            this.fileList.Location = new System.Drawing.Point(3, 16);
            this.fileList.Name = "fileList";
            this.fileList.Size = new System.Drawing.Size(440, 224);
            this.fileList.TabIndex = 0;
            this.fileList.UseCompatibleStateImageBehavior = false;
            this.fileList.View = System.Windows.Forms.View.Details;
            this.fileList.SelectedIndexChanged += new System.EventHandler(this.fileList_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "";
            this.columnHeader1.Width = 426;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(447, 295);
            this.Controls.Add(this.filesGroup);
            this.Controls.Add(this.networkGroup);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MUFT";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.networkGroup.ResumeLayout(false);
            this.networkGroup.PerformLayout();
            this.filesGroup.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label IPLabel;
        private System.Windows.Forms.TextBox IPTextBox;
        private System.Windows.Forms.Label portLabel;
        private System.Windows.Forms.TextBox portTextBox;
        private System.Windows.Forms.RadioButton radioClient;
        private System.Windows.Forms.RadioButton radioServer;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox networkGroup;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.GroupBox filesGroup;
        private System.Windows.Forms.ListView fileList;
        private System.Windows.Forms.ColumnHeader columnHeader1;
    }
}