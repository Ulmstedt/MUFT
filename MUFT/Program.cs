using System;

namespace MUFT
{
    class Program
    {
        //static int chunk_size = 20; // Number of bytes to read/write at a time

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


            //string src_path = @"C:\Users\Sehnsucht\Desktop\StreamTest\Original.txt";
            //string dst_path = @"C:\Users\Sehnsucht\Desktop\StreamTest\Copy.txt";

            //BinaryReader src_br = new BinaryReader(File.OpenRead(src_path));
            //BinaryWriter dst_br = new BinaryWriter(File.OpenWrite(dst_path));

            //Byte[] bytes = new Byte[chunk_size];
            //int bytes_total = 0;
            //int bytes_read = 0;

            //do
            //{
            //    bytes_read = src_br.Read(bytes, 0, chunk_size); // Read bytes
            //    dst_br.Write(bytes, 0, bytes_read); // Write bytes
            //    bytes_total += bytes_read;
            //} while (bytes_read != 0);


            //src_br.Close();
            //dst_br.Close();

            //Console.WriteLine(bytes_total + " bytes successfully copied.");
            //Console.ReadKey(); // Pause
        }
    }
}
