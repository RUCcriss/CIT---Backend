
using System.Net.Sockets;
using System.Net;
using System.Text;
using Server;

int port = 5000;

var server = new EchoServer(port);

server.Run();