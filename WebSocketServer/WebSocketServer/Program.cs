using WebSocketSharp;
using WebSocketSharp.Server;

public class Program
{
    public static void Main(string[] args)
    {
        var wssv = new WebSocketServer("ws://127.0.0.1:7890");
        wssv.AddWebSocketService<DefaultGame>("/DefaultGame");
        wssv.Start();
        Console.ReadKey(true);
        wssv.Stop();
    }
}