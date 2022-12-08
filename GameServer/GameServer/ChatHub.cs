
using Microsoft.AspNetCore.SignalR;

public class ChatHub : Hub
{
    public async Task SendSomeStr(string message)
    {
        await Task.Run(() => Console.WriteLine(message));
    }
}