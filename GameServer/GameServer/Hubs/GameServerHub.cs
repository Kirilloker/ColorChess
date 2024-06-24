using ColorChessModel;
using GameServer.Database;
using GameServer.GameServer.GameServerModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace GameServer.GameServer.Hubs
{
    [Authorize]
    public class GameServerHub : Hub
    {
        public async Task FindRoom(string _gameMode, string _numOfPlayers)
        {
            await Task.Run(async () =>
            {
                int playerId = GetPlayerId();
                var (numOfPlayers, gameMode, state) = ExtractRoomInfo(_gameMode, _numOfPlayers);

                DB.IDK_how_fix_this_bug();
                AddLog(LogEventType.SearchGame, playerId, "want to play:" + gameMode + " " + numOfPlayers);

                bool roomIsReadyForStart = GameLobby.TryFindRoom(playerId, gameMode, numOfPlayers, out Map? startGameState);

                if (roomIsReadyForStart == true && startGameState != null)
                    await NotifyPlayerOfStartGame(numOfPlayers, playerId, gameMode, startGameState);
            });
        }

        public async Task SendOtherPlayerStep(string step)
        {
            await Task.Run(async () =>
            {
                int playerId = GetPlayerId();

                List<int> players = GameLobby.GetAllPlayersInRoomByPlayerId(playerId);

                GameLobby.ApplyStep(playerId, step);

                foreach (var player in players)
                    if (player != playerId)
                        await Clients.User(player.ToString()).SendAsync("ServerSendStep", step);
            });
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        //TODO: If user leave the room (max player > 2) when it's not full, it can break
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await Task.Run(async () =>
            {
                int playerIdLeaved = GetPlayerId();

                AddLog(LogEventType.SurrenderGame, playerIdLeaved, "player leave the game");

                List<int> playersId = GameLobby.GetAllPlayersInRoomByPlayerId(playerIdLeaved);

                if (playersId != null)
                {
                    GameLobby.PlayerLeftTheGame(playerIdLeaved);

                    foreach (int playerId in playersId)
                        if (playerId != playerIdLeaved)
                            await Clients.User(playerId.ToString()).SendAsync("ServerEndGame");
                }
            });

            await base.OnDisconnectedAsync(exception);
        }


        private async Task NotifyPlayerOfStartGame(int numOfPlayers, int playerId, GameModeType gameMode, Map startGameState)
        {
            await Task.Run(async () =>
            {
                List<int> players = GameLobby.GetAllPlayersInRoomByPlayerId(playerId);

                AddLog(LogEventType.StartGame, players.ToList(), "start game:" + gameMode + " " + numOfPlayers);

                for (int i = 0; i < numOfPlayers; i++)
                {
                    await Clients.User(players[i].ToString()).
                        SendAsync("ServerStartGame", JSONConverter.ConvertToJSON(startGameState.ConvertMapToPlayer(i)));
                }
            });
        }

        private static (int numOfPlayre, GameModeType, bool state) ExtractRoomInfo(string _gameMode, string _numOfPlayers)
        {
            bool state = true;

            if (int.TryParse(_numOfPlayers, out int numOfPlayers) == false)
                throw new Exception($"Error Find room, can't parse numOfPlayers: {numOfPlayers}");

            GameModeType gameMode = _gameMode switch
            {
                "Default" => GameModeType.Default,
                "Rating" => GameModeType.Rating,
                _ => GameModeType.Default
            };

            return (numOfPlayers, gameMode, state);
        }

        private int GetPlayerId()
        {
            if (int.TryParse(Context.UserIdentifier, out int playerId) == false)
                throw new Exception($"Error Find room, can't parse user id: {Context.UserIdentifier}");

            return playerId;
        }

        private static void AddLog(LogEventType logEventType, List<int> usersId, string description)
        {
            LogEvent logEvent = new()
            {
                Date = DateTime.Now,
                Type_Event = logEventType,
                UsersId = usersId,
                Description = description
            };

            DB.AddLogEvent(logEvent);
        }

        private static void AddLog(LogEventType logEventType, int userId, string description)
        {
            AddLog(logEventType, new List<int>() { userId }, description);
        }
    }
}