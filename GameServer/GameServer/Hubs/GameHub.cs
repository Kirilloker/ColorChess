using Microsoft.AspNetCore.SignalR;
using GameServer.GameServer;
using Microsoft.AspNetCore.Authorization;


[Authorize]
public class GameHub : Hub
{
    public async Task SendSomeStr(string message)
    {
        await Task.Run(() => Console.WriteLine(message));
    }

    public async Task FindRoom(string gameMode)
    {
        await Task.Run(() =>
        {

        });    
    }

    public async Task SendPlayerStep(string step)
    {
        await Task.Run(() =>
        {
            Console.WriteLine(step);

        });
    }

    public override async Task OnConnectedAsync()
    {
        await Task.Run(() =>
        {
            Console.WriteLine($"User ({Context.UserIdentifier}) connected to gameHub");

        });

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await Task.Run(() =>
        {
            if(exception == null) Console.WriteLine($"User ({Context.UserIdentifier}) disconnected from gameHub");

            else Console.WriteLine(exception);

        });
        await base.OnDisconnectedAsync(exception);
    }
}