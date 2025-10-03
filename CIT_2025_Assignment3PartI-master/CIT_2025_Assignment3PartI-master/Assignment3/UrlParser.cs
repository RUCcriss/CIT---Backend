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

        public static bool ParseUrl(string url)
        {
            return true;
        }
    }
}