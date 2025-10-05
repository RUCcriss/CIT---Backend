using System.Net.Sockets;
using System.Net;
using System;
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
            var stream = client.GetStream();

            var msg = "Hello from server";
            byte[] data = Encoding.UTF8.GetBytes(msg);
            stream.Write(data, 0, data.Length);

            client.Close();
        }
    }
}
