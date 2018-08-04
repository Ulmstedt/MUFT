using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace MUFT
{
    abstract class FileTransferConnection
    {
        protected TcpClient connection = null;
        protected int port; // Port to listen on/connect to

        private int chunk_size = 4096;

        public List<SimpleFileInfo> FileList { get; set; }
        public int NumFiles { get; set; }
        public long TotalSize { get; set; }


        abstract public void Connect();


        // Send all files in fileList
        public void SendFiles(BackgroundWorker bgw)
        {
            BinaryWriter bw = new BinaryWriter(connection.GetStream());
            SendMetaData(bw);
            foreach (SimpleFileInfo fileInfo in FileList)
            {
                SendFile(fileInfo, bw, bgw);
            }
        }

        // Receive files according to numFiles and totalSize
        public void ReceiveFiles(string path, ProgressBar currentProgress, ProgressBar totalProgress)
        {
            BinaryReader br = new BinaryReader(connection.GetStream());
            RecvMetaData(br);
            for (int i = 0; i < NumFiles; ++i)
            {
                RecvFile(path, br, currentProgress, totalProgress);
            }
        }

        // Send meta data about the transfer
        public void SendMetaData(BinaryWriter bw)
        {
            bw.Write(NumFiles);
            bw.Write(TotalSize);
        }

        // Receive meta data about the transfer
        public void RecvMetaData(BinaryReader br)
        {
            NumFiles = br.ReadInt32();
            TotalSize = br.ReadInt64();
        }

        // Send a file over the socket
        private void SendFile(SimpleFileInfo fileInfo, BinaryWriter bw, BackgroundWorker bgw)
        {
            bw.Write(fileInfo.Size);
            bw.Write(fileInfo.Name);

            BinaryReader br = new BinaryReader(File.OpenRead(fileInfo.Path));

            long bytesSent = 0;
            Byte[] bytes = new Byte[chunk_size]; // Buffer

            int lastReportedCurr = 0;
            while (bytesSent < fileInfo.Size)
            {
                int bytes_read = br.Read(bytes, 0, chunk_size);
                bw.Write(bytes, 0, bytes_read);
                bytesSent += bytes_read;

                // Update progress bar
                int curr = (int)((float)bytesSent / fileInfo.Size * 100);
                if (curr - lastReportedCurr > 1) // Only update changes of 1% or more
                {
                    bgw.ReportProgress(0, new ProgressArgs(curr, 0));
                    lastReportedCurr = curr;
                }

            }

            br.Close();
        }

        // Recieve a file from the socket
        private void RecvFile(string path, BinaryReader br, ProgressBar currentProgress, ProgressBar totalProgress)
        {
            long fileSize = br.ReadInt64();
            string fileName = br.ReadString();

            BinaryWriter bw = new BinaryWriter(File.OpenWrite(path + @"\" + fileName));

            long bytesReceived = 0;
            Byte[] bytes = new Byte[chunk_size]; // Buffer

            while (bytesReceived < fileSize)
            {
                int bytes_read = br.Read(bytes, 0, chunk_size);
                bytesReceived += bytes_read;
                bw.Write(bytes, 0, bytes_read);
            }

            bw.Close();
        }

        // Close the connection
        public void Close()
        {
            if (connection != null)
            {
                connection.Close();
            }
        }
    }
}
