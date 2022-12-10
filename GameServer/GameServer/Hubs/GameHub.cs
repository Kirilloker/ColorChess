using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using ColorChessModel;


[Authorize]
public class GameHub : Hub
{
    public async Task FindRoom(string _gameMode)
    {
        await Task.Run( async () =>
        {
            GameMode gameMode;
            int userId = int.Parse(Context.UserIdentifier);

            switch (_gameMode)
            {
                case "Default":
                {
                    gameMode = GameMode.Default;

                    DB.AddUserInLobby(userId, gameMode);
                    int opponentId = DB.SearchOpponent(userId);
                    if (opponentId != -1)
                    {
                        GameStateBuilder builder = new GameStateBuilder();
                        builder.SetDefaultOnlineGameState();
                        Map gameState = builder.CreateGameState();

                        DB.AddRoom(userId, opponentId, JsonConverter.ConvertToJSON(gameState));
                        DB.DeleteUserInLobby(userId);
                        DB.DeleteUserInLobby(opponentId);

                        await Clients.User(Context.UserIdentifier).SendAsync("ServerStartGame", JsonConverter.ConvertToJSON(gameState.ConvertMapToPlayer(0)));
                        await Clients.User(opponentId.ToString()).SendAsync("ServerStartGame", JsonConverter.ConvertToJSON(gameState.ConvertMapToPlayer(1)));
                    }
                }
                    break;
                default:
                    break;
            }
        });    
    }

    public async Task SendPlayerStep(string step)
    {
        await Task.Run(() =>
        {
            int playerId = int.Parse(Context.UserIdentifier);
            Room room = DB.GetRoom(playerId);
            GameMode gameMode 
        switch (_gameMode)
        {
            case "Default":
            {
                if (room.User1Id == playerId)
                {

                }
                else
                {

                }
            }
                break;
            default:
                break;
        };
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
            DB.DeleteUserInLobby(int.Parse(Context.UserIdentifier));
            if (exception == null) Console.WriteLine($"User ({Context.UserIdentifier}) disconnected from gameHub");

            else Console.WriteLine(exception);

        });
        await base.OnDisconnectedAsync(exception);
    }
}