using System.Collections.Generic;
using System.Linq;
using System;
using System.Text.Json;

namespace Utility
{
    //request klasse
    public class Request
    {
        public string? Method { get; set; }
        public string? Path { get; set; }
        public string? Date { get; set; }
        public string? Body { get; set; }
    }

    //respone klasse
    public class Response
    {
        public string Status { get; set; } = "";
        public string? Body { get; set; } = null;
    }

    public class RequestValidator
    {
        private List<string> acceptedM = new List<string> { "read", "create", "echo", "delete", "update" };

        public Response ValidateRequest(Request request)
        {
            List<string> Errors = new List<string>();

            //Check if method is missing --> else do validation of crud request
            if (string.IsNullOrEmpty(request.Method)) Errors.Add("missing method");

            //Date
            long dateValue; //needed for TryParse
            if (request.Date == null) Errors.Add("missing date");
            else if (!long.TryParse(request.Date, out dateValue)) Errors.Add("illegal date"); //Cant parse from string
            else if (long.Parse(request.Date) < 0) Errors.Add("illegal date"); //UNIX time does not go negative

            //CRUD might as well start with parseUrl
            if (string.IsNullOrEmpty(request.Path)) Errors.Add("missing path");
            UrlParser urlParser = new UrlParser();
            bool parseResult = urlParser.ParseUrl(request.Path);
            if (!parseResult) Errors.Add("bad request");

            switch (request.Method)
            {
                case "create":
                    if (urlParser.HasId) Errors.Add("bad request");
                    if (string.IsNullOrEmpty(request.Body)) Errors.Add("missing body");
                    if (!hasJsonBody(request.Body)) Errors.Add("illegal body");
                    break;
                case "read":
                    break;
                case "update":
                    if (!urlParser.HasId) Errors.Add("bad request");
                    if (string.IsNullOrEmpty(request.Body)) Errors.Add("missing body");
                    if (!hasJsonBody(request.Body)) Errors.Add("illegal body");
                    break;
                case "delete":
                    if (!urlParser.HasId) Errors.Add("bad request");
                    break;
                case "echo":
                    if (String.IsNullOrEmpty(request.Body)) Errors.Add("missing body");
                    break;
                default:
                    Errors.Add("illegal method");
                    break;
            }

            //Return response
            if (Errors.Count > 0)
            {
                return new Response
                {
                    Status = "4 " + string.Join(", ", Errors),
                    Body = null
                };
            }

            return new Response
            {
                Status = "1 Ok",
                Body = null
            };
        }

        private static bool hasJsonBody(string? body)
        {
            if (body == null) return false; //to get rid of possible null reference warning (warning CS8604)

            try
            {
                JsonDocument.Parse(body);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
