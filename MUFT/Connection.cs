using System;
using System.IO;
using System.Net.Sockets;

namespace MUFT
{
    abstract class Connection
    {
        protected TcpClient connection = null;
        protected int port; // Port to listen on/connect to

        private int chunk_size = 1024;

        abstract public void Connect();

        // Send a file over the socket
        public void SendFile(string path)
        {
            // Get file size
            FileInfo fi = new FileInfo(path);
            long fileSize = fi.Length;
            Console.WriteLine("File size: " + fileSize);

            BinaryReader br = new BinaryReader(File.OpenRead(path)); // Open stream from file to be sent
            NetworkStream ns = connection.GetStream();

            // Send file metadata (size)
            Byte[] fileSizeBytes = BitConverter.GetBytes(fileSize);
            ns.Write(fileSizeBytes, 0, sizeof(long));

            int bytesSent = 0;
            Byte[] bytes = new Byte[chunk_size]; // Buffer

            while(bytesSent < fileSize)
            {
                int bytes_read = br.Read(bytes, 0, chunk_size);
                bytesSent += bytes_read;
                ns.Write(bytes, 0, bytes_read); // Write to network socket
            }

            ns.Close();
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

            Console.WriteLine("Size of file to be received: " + fileSize);

            int bytesReceived = 0;
            Byte[] bytes = new Byte[chunk_size]; // Buffer

            while (bytesReceived < fileSize)
            {
                int bytes_read = ns.Read(bytes, 0, chunk_size);
                bytesReceived += bytes_read;
                bw.Write(bytes, 0, bytes_read);
            }

            ns.Close();
            bw.Close();
        }

        // Close the connection
        public void Close()
        {
            if(connection != null)
            {
                connection.Close();
            }
        }
    }
}
