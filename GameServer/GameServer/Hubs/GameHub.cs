using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using ColorChessModel;


[Authorize]
public class GameHub : Hub
{
    public async Task SendSomeStr(string message)
    {
        await Task.Run(() => Console.WriteLine(message));
    }

    public async Task FindRoom(GameMode gameMode)
    {
        await Task.Run(() =>
        {
            DB.AddUserInLobby(int.Parse(Context.UserIdentifier), gameMode);
            int opponentId = DB.SearchOpponent(int.Parse(Context.UserIdentifier));
            if (opponentId != -1)
            {
                GameStateBuilder builder = new GameStateBuilder();
                builder.SetDefaultOnlineGameState();
                Map gameState = builder.CreateGameState();

                DB.AddRoom(int.Parse(Context.UserIdentifier), opponentId, JsonConverter.ConvertToJSON(gameState));
                DB.DeleteUserInLobby(int.Parse(Context.UserIdentifier));
                DB.DeleteUserInLobby(opponentId);

                Clients.User(Context.UserIdentifier).SendAsync("", JsonConverter.ConvertToJSON(gameState.ConvertMapToPlayer(0)));
                Clients.User(opponentId.ToString()).SendAsync("", JsonConverter.ConvertToJSON(gameState.ConvertMapToPlayer(1)));
            }    
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