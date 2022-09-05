using WebSocketSharp;
using WebSocketSharp.Server;

public class Program
{
    public static void Main(string[] args)
    {
        var wssv = new WebSocketServer("ws://192.168.0.42:7890");
        wssv.AddWebSocketService<DefaultGame>("/DefaultGame");
        wssv.Start();
        Console.ReadKey(true);
        wssv.Stop();
    }
}