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
                if (validatorResponse.Status != "1 ok")
                {
                    // Da requestValidator.ValidateRequest() returnerer fejlkoder for malforme requests, kan vi som udgangspunkt blot sende fejlkoderne direkte tilbage til client.
                    Util.sendResponse(validatorResponse, client);
                }
                else
                {
                    //Ellers bør requesten behandles, idet ValidateRequest() blot returnerer "1 OK" hvis requesten valideres. (det er jo ikke det rette at returnere til clienten. så vi skal lave en anden response når request behandles)
                    Console.WriteLine(validatorResponse.Status);
                    Util.sendResponse(new Response { Status = "6 Error" }, client);
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"HandleClient caught an IOException: {ex}"); //.Message as just ex is quite verbose
            }
            catch (Exception ex)
            {
                Console.WriteLine($"HandleClient caught an Exception: {ex}"); //.Message as just ex is quite verbose
            }
            finally
            {
                client.Close();
            }
        }

    }
}
