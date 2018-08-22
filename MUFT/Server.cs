using System;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace MUFT
{
    class Server : FileTransferConnection
    {
        TcpListener listener;

        public Server(int port) : base()
        {
            this.port = port;

            listener = new TcpListener(IPAddress.Any, this.port);
            // Start listening for client requests
            Console.WriteLine("Listening for connections...");
            listener.Start();
        }

        public override void Connect(BackgroundWorker bgw)
        {
            try
            {
                while (true)
                {
                    if (bgw.CancellationPending)
                    {
                        listener.Stop();
                        return;
                    }
                    else if (listener.Pending())
                    {
                        connection = listener.AcceptTcpClient();
                        Console.WriteLine("Connected!");
                        listener.Stop();
                        return;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
