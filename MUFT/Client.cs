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
                connection = new TcpClient(this.ip, this.port);
                ns = connection.GetStream();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
    }
}
