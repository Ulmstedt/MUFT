using System;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace MUFT
{
    class Server : FileTransferConnection
    {
        public Server(int port) : base()
        {
            this.port = port;
        }

        public override void Connect()
        {
            try
            {
                TcpListener listener = new TcpListener(IPAddress.Any, this.port);

                // Start listening for client requests
                Console.WriteLine("Listening for connections...");
                listener.Start();
                connection = listener.AcceptTcpClient();
                Console.WriteLine("Connected!");
                listener.Stop();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
