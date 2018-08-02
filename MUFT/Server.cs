using System;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace MUFT
{
    class Server : FileTransferConnection
    {
        public Server(int port)
        {
            this.port = port;
        }

        public override void Connect()
        {
            try
            {
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");

                TcpListener listener = new TcpListener(localAddr, this.port);

                // Start listening for client requests
                listener.Start();

                connection = listener.AcceptTcpClient();
                ns = connection.GetStream();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
    }
}
