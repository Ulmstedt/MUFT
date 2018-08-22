using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Timers;
using System.Windows.Forms;

namespace MUFT
{
    abstract class FileTransferConnection
    {
        protected TcpClient connection = null;
        protected int port; // Port to listen on/connect to

        private const int chunkSize = 16384;
        private const int movingAvgSize = 5;

        public List<SimpleFileInfo> FileList { get; set; }
        public int NumFiles { get; set; }
        public long TotalSize { get; set; }

        private long totalBytesTransfered;
        private long previousTotalBytesTransfered;

        private List<long> recentSpeeds;
        public long CurrentSpeed
        {
            get
            {
                if (recentSpeeds.Count != 0)
                {
                    return (long)recentSpeeds.Average();
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                recentSpeeds.Add(value);
                if (recentSpeeds.Count > movingAvgSize)
                    recentSpeeds.RemoveAt(0);
            }
        }

        public TimeSpan TimeRemaining { get; set; }

        System.Timers.Timer speedTimer;

        abstract public void Connect(BackgroundWorker bgw);

        protected FileTransferConnection()
        {
            recentSpeeds = new List<long>();
            InitializeSpeedTimer();
        }

        private void InitializeSpeedTimer()
        {
            speedTimer = new System.Timers.Timer(1000);
            speedTimer.Elapsed += speedTimer_Tick;
            speedTimer.AutoReset = true;
        }

        public void speedTimer_Tick(Object source, ElapsedEventArgs e)
        {
            CurrentSpeed = totalBytesTransfered - previousTotalBytesTransfered;
            if(CurrentSpeed != 0)
                TimeRemaining = TimeSpan.FromSeconds((TotalSize - totalBytesTransfered) / CurrentSpeed);
            previousTotalBytesTransfered = totalBytesTransfered;
        }

        /*
         * Recursively adds all files in path to the FileList to be transferred.
         * @prefix prefix of the files name (makes sure that the received file is added to the correct folder)
         */
        private void AddFilesInDir(string path, string prefix)
        {
            try
            {
                foreach (string f in Directory.GetFiles(path))
                {
                    FileInfo fi = new FileInfo(f);
                    SimpleFileInfo file = new SimpleFileInfo(fi.FullName, prefix + fi.Name, fi.Length, false);
                    FileList.Add(file);
                    NumFiles++;
                }

                foreach (string d in Directory.GetDirectories(path))
                {
                    string newPrefix = prefix + d.Split('\\').Last() + "\\";
                    AddFilesInDir(d, newPrefix);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        // Send all files in fileList
        public void SendFiles(BackgroundWorker bgw)
        {
            Connect(bgw);
            if (connection == null)
                return;

            BinaryWriter bw = null;
            try
            {
                totalBytesTransfered = 0;
                bw = new BinaryWriter(connection.GetStream());

                // Add files in directories
                foreach (SimpleFileInfo fileInfo in FileList.ToList())
                {
                    if (fileInfo.IsDirectory)
                    {
                        AddFilesInDir(fileInfo.Path, "");
                        FileList.Remove(fileInfo);
                    }
                }

                if (!SendMetaData(bw))
                {
                    MessageBox.Show("Failed to send meta data");
                    return;
                }

                speedTimer.Start();

                foreach (SimpleFileInfo fileInfo in FileList)
                {
                    if (bgw.CancellationPending) return;
                    SendFile(fileInfo, bw, bgw);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }
            finally
            {
                if (bw != null)
                {
                    bw.Close();
                }
                Close();
                speedTimer.Stop();
            }
        }

        // Receive files according to numFiles and totalSize
        public void ReceiveFiles(string path, BackgroundWorker bgw)
        {
            Connect(bgw);
            if (connection == null)
                return;

            BinaryReader br = null;
            try
            {
                br = new BinaryReader(connection.GetStream());
                if (!RecvMetaData(br))
                {
                    MessageBox.Show("Failed to receive meta data");
                    return;
                }

                speedTimer.Start();

                for (int i = 0; i < NumFiles; ++i)
                {
                    if (bgw.CancellationPending) return;
                    RecvFile(path, br, bgw);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }
            finally
            {
                if (br != null)
                {
                    br.Close();
                }
                Close();
                speedTimer.Stop();
            }

        }

        // Send a file over the socket
        private void SendFile(SimpleFileInfo fileInfo, BinaryWriter bw, BackgroundWorker bgw)
        {
            BinaryReader br = null;
            FileStream fs = null;
            try
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();
                string checksum = Utilities.CalculateMD5(fileInfo.Path);
                watch.Stop();
                Console.WriteLine("Checksum calculation took {0}", watch.Elapsed); ;

                fs = File.OpenRead(fileInfo.Path);
                br = new BinaryReader(fs);

                bw.Write(fileInfo.Size);
                bw.Write(fileInfo.Name);
                bw.Write(checksum);

                Console.WriteLine("Sending " + fileInfo.Name + ", " + fileInfo.Size);

                long bytesSent = 0;
                Byte[] bytes = new Byte[chunkSize]; // Buffer

                int lastReportedCurr = 0;
                while (bytesSent < fileInfo.Size)
                {
                    if (bgw.CancellationPending) return;

                    int bytes_read = br.Read(bytes, 0, chunkSize);
                    bw.Write(bytes, 0, bytes_read);

                    bytesSent += bytes_read;
                    totalBytesTransfered += bytes_read;

                    // Update progress bar
                    int curr = (int)((float)bytesSent / fileInfo.Size * 100);
                    if (curr - lastReportedCurr > 1 || curr == 100) // Only update changes of 1% or more
                    {
                        int total = (int)((float)totalBytesTransfered / TotalSize * 100);
                        bgw.ReportProgress(0, new ProgressArgs(null, curr, total, totalBytesTransfered, TotalSize, CurrentSpeed, TimeRemaining));
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
                    br.Close();
                if (fs != null)
                    fs.Close();

            }
        }

        // Recieve a file from the socket
        private void RecvFile(string path, BinaryReader br, BackgroundWorker bgw)
        {
            string fullPath = "", checksum = "", fileName = "";
            BinaryWriter bw = null;
            FileStream fs = null;

            try
            {
                long fileSize = br.ReadInt64();
                fileName = br.ReadString();
                checksum = br.ReadString();

                fullPath = path + @"\" + fileName;
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                Console.WriteLine("Receiving " + fileName + ", " + fileSize);

                fs = File.OpenWrite(fullPath);
                bw = new BinaryWriter(fs);

                long bytesReceived = 0;
                long bytesLeft = fileSize;
                Byte[] bytes = new Byte[chunkSize]; // Buffer

                bool reportedFile = false;
                int lastReportedCurr = 0;
                while (bytesLeft > 0)
                {
                    if (bgw.CancellationPending)
                    {
                        fs.Close();
                        bw.Close();
                        File.Delete(fullPath);
                        return;
                    }

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
                        SimpleFileInfo fileInfo = null;
                        if (!reportedFile)
                        {
                            fileInfo = new SimpleFileInfo(fullPath, fileName, fileSize, false);
                            reportedFile = true;
                        }
                        int total = (int)((float)totalBytesTransfered / TotalSize * 100);
                        bgw.ReportProgress(0, new ProgressArgs(fileInfo, curr, total, totalBytesTransfered, TotalSize, CurrentSpeed, TimeRemaining));
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
                    bw.Close();
                if (fs != null)
                    fs.Close();
            }

            // Verify checksum
            string recvChecksum = Utilities.CalculateMD5(fullPath);
            if (checksum != recvChecksum)
                new Thread(() => MessageBox.Show("Warning! Checksum of " + fileName + " did not match expected checksum.")).Start();
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
                MessageBox.Show("SendMetaData: " + e.Message);
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
