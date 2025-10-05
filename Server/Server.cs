using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using Utility;

namespace Server
{
    public class Server
    {
        TcpListener server;

        public Server(int port)
        {
            this.server = new TcpListener(IPAddress.Loopback, port);
            server.Start(); //starts the listener
            Console.WriteLine($"Server started on port {port}");
        }

        public void Run()
        {
            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("Client connected");

                Thread clientThread = new Thread(() => HandleClient(client));
                clientThread.Start();
            }
        }

        private static void HandleClient(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            Console.WriteLine(parseStreamToString(stream));
            client.Close();
        }

        private static string parseStreamToString(NetworkStream stream)
        {
            byte[] resp = new byte[2048];
            using (var memStream = new MemoryStream())
            {
                int bytesread = 0;
                do
                {
                    bytesread = stream.Read(resp, 0, resp.Length);
                    memStream.Write(resp, 0, bytesread);

                } while (bytesread == 2048);

                return Encoding.UTF8.GetString(memStream.ToArray());
            }
        }
    }
}
