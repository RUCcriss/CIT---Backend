using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using Utility;
using System.Text.Json;

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

                Console.WriteLine("Req meth: " + request.Method);
                Console.WriteLine("Reg bod: " + request.Body);
                Console.WriteLine("Status: " + validatorResponse.Status);
                Console.WriteLine("Body: " + validatorResponse.Body);

                if (string.IsNullOrEmpty(request.Body))
                {
                    Console.WriteLine("Request body as string is NULL or empty");
                }
                else
                {
                    try
                    {
                        String JsonParse = JsonDocument.Parse(request.Body).ToString();
                        Console.WriteLine("Succes! JsonParse of body");
                        Console.WriteLine(JsonParse);
                    }
                    catch (JsonException)
                    {
                        Console.WriteLine("JsonParse failed");
                    }                    
                }
                Console.WriteLine(" ");

               


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
