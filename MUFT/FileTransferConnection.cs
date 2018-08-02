using System;
using System.IO;
using System.Net.Sockets;

namespace MUFT
{
    abstract class FileTransferConnection
    {
        protected TcpClient connection = null;
        protected int port; // Port to listen on/connect to
        protected NetworkStream ns = null;

        private int chunk_size = 1024;

        abstract public void Connect();

        public int NumFiles { get; set; }
        public long TotalSize { get; set; }

        // Send meta data about the transfer
        public void SendMetaData()
        {
            Byte[] numFilesBytes = BitConverter.GetBytes(NumFiles);
            Byte[] totalSizeBytes = BitConverter.GetBytes(TotalSize);

            ns.Write(numFilesBytes, 0, sizeof(int));
            ns.Write(totalSizeBytes, 0, sizeof(long));
        }

        // Receive meta data about the transfer
        public void RecvMetaData()
        {
            Byte[] numFilesBytes = new Byte[sizeof(int)];
            Byte[] totalSizeBytes = new Byte[sizeof(long)];

            ns.Read(numFilesBytes, 0, sizeof(int));
            ns.Read(totalSizeBytes, 0, sizeof(long));

            NumFiles = BitConverter.ToInt32(numFilesBytes, 0);
            TotalSize = BitConverter.ToInt64(totalSizeBytes, 0);
        }

        // Send a file over the socket
        public void SendFile(SimpleFileInfo fileInfo)
        {
            BinaryReader br = new BinaryReader(File.OpenRead(fileInfo.Path)); // Open stream from file to be sent

            // Send file metadata (size)
            Byte[] fileSizeBytes = BitConverter.GetBytes(fileInfo.Size);
            ns.Write(fileSizeBytes, 0, sizeof(long));

            int bytesSent = 0;
            Byte[] bytes = new Byte[chunk_size]; // Buffer

            while (bytesSent < fileInfo.Size)
            {
                int bytes_read = br.Read(bytes, 0, chunk_size);
                bytesSent += bytes_read;
                ns.Write(bytes, 0, bytes_read); // Write to network socket
            }

            br.Close();
        }

        // Recieve a file from the socket
        public void RecvFile(string path)
        {
            BinaryWriter bw = new BinaryWriter(File.OpenWrite(path)); // Open stream to file to be received
            NetworkStream ns = connection.GetStream();

            long fileSize;

            // Receive file metadata (size)
            Byte[] fileSizeBytes = new Byte[sizeof(long)];
            ns.Read(fileSizeBytes, 0, sizeof(long));
            fileSize = BitConverter.ToInt64(fileSizeBytes, 0);

            int bytesReceived = 0;
            Byte[] bytes = new Byte[chunk_size]; // Buffer

            while (bytesReceived < fileSize)
            {
                int bytes_read = ns.Read(bytes, 0, chunk_size);
                bytesReceived += bytes_read;
                bw.Write(bytes, 0, bytes_read);
            }

            bw.Close();
        }

        // Close the connection
        public void Close()
        {
            if(ns != null)
            {
                ns.Close();
            }

            if (connection != null)
            {
                connection.Close();
            }
        }
    }
}
