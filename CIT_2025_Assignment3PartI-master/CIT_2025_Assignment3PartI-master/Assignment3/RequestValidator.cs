using System.Collections.Generic;
using System.Linq;
using System;

namespace Assignment3
{
#nullable enable

    //request klasse
    public class Request
    {
        public string? Method { get; set; }
        public string? Path { get; set; }
        public long? Date { get; set; }
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

        public Response ValidReq(Request request)
        {
            List<string> Errors = new List<string>();

            if (string.IsNullOrEmpty(request.Method))
            {
                Errors.Add("missing method");
            }
            else if (!acceptedM.Contains(request.Method))
            {
                Errors.Add("illegal method");
            }

            if (string.IsNullOrEmpty(request.Path))
            {
                Errors.Add("missing path");
            }
            if (request.Date == null)
            {
                Errors.Add("missing date");
                if (request.Date > 0)
                {
                    Errors.Add("illegal date");
                }
            }


            if (request.Method != "read")
            {
                if (string.IsNullOrEmpty(request.Body))
                {
                    Errors.Add("missing body");
                }
            }

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
