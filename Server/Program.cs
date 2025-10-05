namespace Server;

internal class Program
{
    static int port = 5000;

    private static void Main(string[] args)
    {
        Server server = new Server(port);
    }
}
