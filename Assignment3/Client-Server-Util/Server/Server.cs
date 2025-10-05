using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server;
public class EchoServer
{

    TcpListener _server;

    public int Port { get; set; }

    public EchoServer(int port)
    {
        Port = port;
    }

    public void Run()
    {
        _server = new TcpListener(IPAddress.Loopback, Port);

        _server.Start();
        Console.WriteLine($"Server started on port {Port}");

        while (true)
        {
            TcpClient client = _server.AcceptTcpClient();
            Console.WriteLine("Client connected");
            HandleClient(client);
        }
    }

    private void HandleClient(TcpClient client)
    {
        var stream = client.GetStream();

        var msg = "Hello form server";

        stream.Write(Encoding.UTF8.GetBytes(msg));

    }




}
