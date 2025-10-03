using System.Collections.Generic;
using System.Linq;
using System;
using System.Text.Json;

namespace Assignment3
{
#nullable enable

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

            //Method
            if (string.IsNullOrEmpty(request.Method)) Errors.Add("missing method");
            else if (!acceptedM.Contains(request.Method)) Errors.Add("illegal method");

            if (request.Method != "read")
            {
                if (string.IsNullOrEmpty(request.Body))
                {
                    Errors.Add("missing body");
                } else
                {
                    try
                    {
                        JsonDocument.Parse(request.Body);
                    }
                    catch (JsonException)
                    {
                        Errors.Add("illegal body");
                    }
                }
            }

            //Path
            if (string.IsNullOrEmpty(request.Path)) Errors.Add("missing path");

            //Date
            long dateValue; //needed for TryParse
            if (request.Date == null) Errors.Add("missing date");
            else if (!long.TryParse(request.Date, out dateValue)) Errors.Add("illegal date"); //Cant parse from string
            else if (long.Parse(request.Date) < 0) Errors.Add("illegal date");

            //Body
            // In cases of method = `create` or `update`
            if (request.Method == "create" || request.Method == "update")
            {
                // FIX: implement a utility function that checks if json is valid
            }
            else if (request.Method == "echo")
            { // if instead echo
              //Should not be a problem. although we might want to check that it is a string (somehow?)
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
    }
}
