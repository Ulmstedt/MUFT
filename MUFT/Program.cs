using System;

namespace MUFT
{
    class Program
    {
        static void Main(string[] args)
        {
            string ip = "127.0.0.1";
            int port = 6113;
            Connection connection;

            Console.WriteLine("(S)erver or (C)lient?");

            while (true)
            {
                Char choice = Console.ReadKey().KeyChar;

                if (choice == 's' || choice == 'S')
                {
                    Console.Clear();
                    connection = new Server(port);
                    connection.Connect();
                    connection.SendFile(@"C:\Users\Sehnsucht\Desktop\StreamTest\Original.txt");
                    connection.Close();

                    break;
                }
                else if (choice == 'c' || choice == 'C')
                {
                    Console.Clear();
                    connection = new Client(ip, port);
                    connection.Connect();
                    connection.RecvFile(@"C:\Users\Sehnsucht\Desktop\StreamTest\Copy.txt");
                    connection.Close();

                    break;
                }
                else
                {
                    Console.WriteLine("\nInvalid choice. Try again.");
                }
            }
        }
    }
}
