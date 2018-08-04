using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace MUFT
{
    abstract class FileTransferConnection
    {
        protected TcpClient connection = null;
        protected int port; // Port to listen on/connect to

        private int chunkSize = 16384;

        public List<SimpleFileInfo> FileList { get; set; }
        public int NumFiles { get; set; }
        public long TotalSize { get; set; }

        private long totalBytesTransfered;

        abstract public void Connect();


        // Send all files in fileList
        public void SendFiles(BackgroundWorker bgw)
        {
            Connect();
            BinaryWriter bw = null;
            try
            {
                totalBytesTransfered = 0;
                bw = new BinaryWriter(connection.GetStream());
                if(!SendMetaData(bw))
                {
                    MessageBox.Show("Failed to send meta data");
                    return;
                }

                foreach (SimpleFileInfo fileInfo in FileList)
                {
                    SendFile(fileInfo, bw, bgw);
                }
            }
            catch (Exception e)
            {
                new Thread(() => MessageBox.Show(e.Message)).Start();
            }
            finally
            {
                if(bw != null)
                {
                    bw.Close();
                }
                Close();
            }
        }

        // Receive files according to numFiles and totalSize
        public void ReceiveFiles(string path, BackgroundWorker bgw)
        {
            Connect();
            BinaryReader br = null;
            try
            {
                br = new BinaryReader(connection.GetStream());
                if(!RecvMetaData(br))
                {
                    MessageBox.Show("Failed to receive meta data");
                    return;
                }

                for (int i = 0; i < NumFiles; ++i)
                {
                    RecvFile(path, br, bgw);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                if(br != null)
                {
                    br.Close();
                }
                Close();
            }

        }

        // Send a file over the socket
        private void SendFile(SimpleFileInfo fileInfo, BinaryWriter bw, BackgroundWorker bgw)
        {
            BinaryReader br = null;
            try
            {
                bw.Write(fileInfo.Size);
                bw.Write(fileInfo.Name);
                Console.WriteLine("Sending " + fileInfo.Name + ", " + fileInfo.Size);

                br = new BinaryReader(File.OpenRead(fileInfo.Path));

                long bytesSent = 0;
                Byte[] bytes = new Byte[chunkSize]; // Buffer

                int lastReportedCurr = 0;
                while (bytesSent < fileInfo.Size)
                {
                    int bytes_read = br.Read(bytes, 0, chunkSize);
                    bw.Write(bytes, 0, bytes_read);

                    bytesSent += bytes_read;
                    totalBytesTransfered += bytes_read;

                    // Update progress bar
                    int curr = (int)((float)bytesSent / fileInfo.Size * 100);
                    if (curr - lastReportedCurr > 1 || curr == 100) // Only update changes of 1% or more
                    {
                        int total = (int)((float)totalBytesTransfered / TotalSize * 100);
                        bgw.ReportProgress(0, new ProgressArgs(curr, total));
                        lastReportedCurr = curr;
                    }

                }
            }
            catch (Exception e)
            {
                new Thread(() => MessageBox.Show(e.Message)).Start();
            }
            finally
            {
                if (br != null)
                {
                    br.Close();
                }
            }
        }

        // Recieve a file from the socket
        private void RecvFile(string path, BinaryReader br, BackgroundWorker bgw)
        {
            BinaryWriter bw = null;
            try
            {
                long fileSize = br.ReadInt64();
                string fileName = br.ReadString();
                Console.WriteLine("Receiving " + fileName + ", " + fileSize);

                bw = new BinaryWriter(File.OpenWrite(path + @"\" + fileName));

                long bytesReceived = 0;
                long bytesLeft = fileSize;
                Byte[] bytes = new Byte[chunkSize]; // Buffer

                int lastReportedCurr = 0;
                while (bytesLeft > 0)
                {
                    int readSize = (int)(chunkSize < bytesLeft ? chunkSize : bytesLeft);
                    int bytes_read = br.Read(bytes, 0, readSize);
                    bw.Write(bytes, 0, bytes_read);

                    bytesReceived += bytes_read;
                    totalBytesTransfered += bytes_read;
                    bytesLeft = fileSize - bytesReceived;


                    // Update progress bar
                    int curr = (int)((float)bytesReceived / fileSize * 100);
                    if (curr - lastReportedCurr > 1 || curr == 100) // Only update changes of 1% or more
                    {
                        int total = (int)((float)totalBytesTransfered / TotalSize * 100);
                        bgw.ReportProgress(0, new ProgressArgs(curr, total));
                        lastReportedCurr = curr;
                    }
                }
            }
            catch (Exception e)
            {
                new Thread(() => MessageBox.Show(e.Message)).Start();
            }
            finally
            {
                if (bw != null)
                {
                    bw.Close();
                }
            }
        }

        // Send meta data about the transfer
        public bool SendMetaData(BinaryWriter bw)
        {
            try
            {
                bw.Write(NumFiles);
                bw.Write(TotalSize);
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
        }

        // Receive meta data about the transfer
        public bool RecvMetaData(BinaryReader br)
        {
            try
            {
                NumFiles = br.ReadInt32();
                TotalSize = br.ReadInt64();
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
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
