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
            try
            {
                NetworkStream stream = client.GetStream();
                Request request = Util.parseStreamToRequest(stream);
                RequestValidator requestValidator = new RequestValidator();
                Response validatorResponse = requestValidator.ValidateRequest(request);
                if (validatorResponse.Status != "1 OK")
                {
                    // Så kan vi svare med response direkte
                    Util.sendResponse(validatorResponse, client);
                }
                else
                {
                    //Ellers behandler vi requesten
                    Console.WriteLine(validatorResponse.Status);
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"HandleClient caught an IOException: {ex.Message}"); //.Message as just ex is quite verbose
            }
            catch (Exception ex)
            {
                Console.WriteLine($"HandleClient caught an Exception: {ex.Message}"); //.Message as just ex is quite verbose
            }
            finally
            {
                client.Close();
            }
        }

    }
}
