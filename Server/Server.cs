using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Collections.Generic;
using Utility;

namespace Server
{
    public class Server
    {
        TcpListener server;
        CategoryService categoryService;

        public Server(int port)
        {
            this.categoryService = new CategoryService();
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

                Thread clientThread = new Thread(() => HandleClient(client, categoryService));
                clientThread.Start();
            }
        }

        private static void HandleClient(TcpClient client, CategoryService categoryService)
        {
            try
            {
                NetworkStream stream = client.GetStream();
                Request request = Util.parseStreamToRequest(stream);
                RequestValidator requestValidator = new RequestValidator();
                UrlParser urlParser = new UrlParser();
                Response validatorResponse = requestValidator.ValidateRequest(request, urlParser);
                if (validatorResponse.Status != "1 Ok")
                {
                    // Da requestValidator.ValidateRequest() returnerer fejlkoder for malforme requests, kan vi som udgangspunkt blot sende fejlkoderne direkte tilbage til client.
                    Util.sendResponse(validatorResponse, client);
                }
                else
                {
                    //Ellers bør requesten behandles, idet ValidateRequest() blot returnerer "1 OK" hvis requesten valideres. (det er jo ikke det rette at returnere til clienten. så vi skal lave en anden response når request behandles)
                    Console.WriteLine(validatorResponse.Status);

                    //switch for different methods
                    switch (request.Method)
                    {
                        case "create":
                            var data = request.Body.FromJson<Dictionary<string, string>>();
                            Console.WriteLine($"Got category: {data["name"]}");
                            int idAdded = categoryService.CreateCategory(data["name"]); // WARN: Assumes this cannot fail
                            Util.sendResponse(new Response() { Status = "2 Created", Body = (new { Cid = idAdded, Name = data["name"] }).ToJson() }, client);
                            break;
                        case "read":
                            break;
                        case "update":
                            break;
                        case "delete":
                            if (categoryService.DeleteCategory(int.Parse(urlParser.Id)))
                            {
                                Util.sendResponse(new Response() { Status = "1 Ok" }, client);
                            }
                            else
                            {
                                Util.sendResponse(new Response() { Status = "5 Not Found" }, client);
                            }
                            break;
                        case "echo":
                            Console.WriteLine(request.Body);
                            Util.sendResponse(new Response() { Status = "1 Ok", Body = request.Body }, client);
                            break;
                        default:
                            break;
                    }

                    //Placeholder response
                    //Util.sendResponse(new Response() { Status = "6 Error" }, client);
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
