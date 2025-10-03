using System;
using System.Linq;


namespace Assignment3
{

    public class UrlParser
    {
        public bool HasId { get; set; }
        public string Id { get; set; }
        public string Path { get; set; }

        public UrlParser()
        {
            HasId = false;
            Id = string.Empty;
            Path = string.Empty;
        }

        public bool ParseUrl(string url)
        {
            string[] strings = url.Split('/');


            if (int.TryParse(strings[strings.Length - 1], out int temp))
            {
                this.HasId = true;
                this.Id = strings[strings.Length - 1];
                this.Path = string.Join("/", strings.Take(strings.Count() - 1));
                return true;
            }
            else
            {
                this.HasId = false;
                this.Id = string.Empty;
                this.Path = url;
            }
            return true;
        }

    }
}
