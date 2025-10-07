using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Utility
{
    public static class Util
    {
        public static string ParseStreamToString(NetworkStream stream)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine($"parseStreamToString caught an Exception: {ex.Message}"); //.Message as just ex is quite verbose
                return String.Empty;
            }
        }

        public static Request parseStreamToRequest(NetworkStream stream)
        {
            try
            {
                string reqString = ParseStreamToString(stream);
                if (String.IsNullOrEmpty(reqString)) return new Request();
                Request? request = JsonSerializer.Deserialize<Request>(reqString, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                if (request == null) return new Request();
                return request;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"parseStreamToRequest caught an Exception: {ex.Message}"); //.Message as just ex is quite verbose
                return new Request();
            }
        }

        public static void sendResponse(Response response, TcpClient client)
        {
            var msg = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(response, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
            Console.WriteLine($"writing response: {JsonSerializer.Serialize(response, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })}");
            client.GetStream().Write(msg, 0, msg.Length);
        }
    }
}
