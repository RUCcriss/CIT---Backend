using System.Collections.Generic;
using System.Linq;
using System;

namespace Assignment3
{
    //request klasse
    public class Request
    {
        public string? Method { get; set; }
        public string? Path { get; set; }
        public long Date { get; set; }
        public string? Body { get; set; }

    }

    //respone klasse
    public class Response
    {
        public string Status { get; set; } = "";
        public string? Body { get; set; } = null;
    }


}
