using System;
using System.Net.Sockets;

namespace MUFT
{
    class Client : Connection
    {
        string ip; // Remote ip

        public Client(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
        }

        public override void Connect()
        {
            try
            {
                Console.WriteLine("Connecting...");
                connection = new TcpClient(this.ip, this.port);
                Console.WriteLine("Connected to " + connection.Client.RemoteEndPoint.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
