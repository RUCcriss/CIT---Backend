using System;
using System.Text.RegularExpressions;

namespace Utility
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

        public bool ParseUrl(string? url)
        {
            //if the path is null, something is for sure wrong
            if (url == null) return false;

            //RegEx for splitting path into group for path and potentially id
            string expression = @"^(\/[^\/]+\/[^\/]+)(?:\/(\d+))?$"; // verbatim used to avoid escape charecters

            Match match = Regex.Match(url, expression);

            if (match.Success)
            {
                Path = match.Groups[1].Value;
                if (match.Groups[2].Success)
                {
                    HasId = true;
                    Id = match.Groups[2].Value;
                }
                return true;
            }
            return false;
        }
    }
}
