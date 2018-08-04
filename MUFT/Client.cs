using System;
using System.Net.Sockets;
using System.Windows.Forms;

namespace MUFT
{
    class Client : FileTransferConnection
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
                Console.WriteLine("Trying to connect");
                connection = new TcpClient(this.ip, this.port);
                Console.WriteLine("Connected!");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
