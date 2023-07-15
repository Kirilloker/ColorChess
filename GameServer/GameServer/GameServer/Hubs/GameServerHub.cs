using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using ColorChessModel;


[Authorize]
public class GameServerHub : Hub
{
    public async Task FindRoom(string _gameMode, string _numOfPlayers)
    {
        await Task.Run(async () =>
        {
            int playerId = int.Parse(Context.UserIdentifier);
            int numOfplayers = int.Parse(_numOfPlayers);
            GameMode gameMode;
            switch (_gameMode)
            {
                case "Default":
                    gameMode = GameMode.Default; break;
                case "Rating":
                    gameMode = GameMode.Rating; break;
                default:
                    gameMode = GameMode.Default; break;
                    break;
            }
            Map StartGameState = GameLobby.FindRoomForPlayerAndStartGame(playerId, gameMode, int.Parse(_numOfPlayers));
            
            if((Object)StartGameState != null)
            {
                List<int> players = GameLobby.GetAllPlayersInRoomWithPlayer(playerId);
                for(int i = 0; i < players.Count; i++)
                {
                    await Clients.User(players[i].ToString()).
                    SendAsync("ServerStartGame", JsonConverter.ConvertToJSON(StartGameState.ConvertMapToPlayer(i)));
                }
            }
        });
    }

    public async Task SendPlayerStep(string step)
    {
        await Task.Run(async () =>
        {
            int playerId = int.Parse(Context.UserIdentifier);
            Map NextGameState = GameLobby.SendPlayerStepToRoomAndApplyIt(playerId, step);

            if ((Object)NextGameState != null)
            {
                List<int> players = GameLobby.GetAllPlayersInRoomWithPlayer(playerId);
                for (int i = 0; i < players.Count; i++)
                {
                    if (players[i] != playerId)
                    {
                        await Clients.User(players[i].ToString()).SendAsync("ServerSendStep", step);
                    }
                }
            }
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
        await Task.Run(async () =>
        {
            int leavedPlayer = int.Parse(Context.UserIdentifier);
            GameLobby.PlayerLeftTheGame(leavedPlayer);
        });
        await base.OnDisconnectedAsync(exception);
    }
}
