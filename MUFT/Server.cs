using System;
using System.Net;
using System.Net.Sockets;

namespace MUFT
{
    class Server : Connection
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

                Console.WriteLine("Waiting for a connection...");

                connection = listener.AcceptTcpClient();
                Console.WriteLine("Connected to " + connection.Client.RemoteEndPoint.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
