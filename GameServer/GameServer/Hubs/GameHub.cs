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
                            builder.SetDefaultHotSeatGameState();
                            Map gameState = builder.CreateGameState();

                            DB.AddRoom(userId, opponentId, JsonConverter.ConvertToJSON(gameState), GameMode.Default);
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
        await Task.Run(async () =>
        {
            Console.WriteLine($"Player({Context.UserIdentifier}) send step");

            int playerId = int.Parse(Context.UserIdentifier);
            Room room = DB.GetRoom(playerId);
            GameMode gameMode = room.GameMode;
            Map map = JsonConverter.ConvertJSONtoMap(room.Map);

            switch (gameMode)
            {
                case GameMode.Default:
                    {
                        if (room.User1Id == playerId)
                        {
                            if (map.NumberPlayerStep == 0)
                            {
                                Map newMap = VerifyAndApplyStep(step, map);
                                if ((Object)newMap != null)
                                {
                                    await Clients.User(room.User2Id.ToString()).SendAsync("ServerSendStep", step);

                                    DB.ChangeRoom(playerId, JsonConverter.ConvertToJSON(newMap));
                                    if (newMap.EndGame)
                                    {
                                        int player1Score = newMap.GetScorePlayer(0);
                                        int player2Score = newMap.GetScorePlayer(1);
                                        //Удаляем комнату и добавляем статистику игры
                                        DB.DeleteRoom(playerId);
                                        DB.AddGameStatistic(10, player1Score, player2Score,
                                            DateTime.Now, GameMode.Default, room.User1Id, room.User2Id);

                                        //Юзер статистика 
                                        UserStatistic player1Stat = DB.GetUserStatistic(room.User1Id);
                                        UserStatistic player2Stat = DB.GetUserStatistic(room.User2Id);

                                        //Меняем рекорд набранных очков
                                        if (player1Stat.MaxScore < player1Score)
                                            DB.ChangeUserStatistic(room.User1Id, AttributeUS.MaxScore, player1Score);
                                        if (player2Stat.MaxScore < player2Score)
                                            DB.ChangeUserStatistic(room.User2Id, AttributeUS.MaxScore, player2Score);

                                        //Если ничья
                                        if (player1Score == player2Score)
                                        {
                                            DB.ChangeUserStatistic(room.User1Id, AttributeUS.Draw, 1);
                                            DB.ChangeUserStatistic(room.User2Id, AttributeUS.Draw, 1);
                                        }
                                        //Если выиграл 1 игрок
                                        else if (player1Score > player2Score)
                                        {
                                            DB.ChangeUserStatistic(room.User1Id, AttributeUS.Win, 1);
                                            DB.ChangeUserStatistic(room.User2Id, AttributeUS.Lose, 1);
                                        }
                                        //Если выиграл 2 игрок
                                        else if (player1Score < player2Score)
                                        {
                                            DB.ChangeUserStatistic(room.User1Id, AttributeUS.Lose, 1);
                                            DB.ChangeUserStatistic(room.User2Id, AttributeUS.Win, 1);
                                        }
                                        // вызвать метод конца игры на клиентах
                                        await Clients.User(room.User1Id.ToString()).SendAsync("ServerEndGame");
                                        await Clients.User(room.User2Id.ToString()).SendAsync("ServerEndGame");
                                    }
                                }
                            }
                            else
                            {
                                //Тут логика, что игрок прислал ход не в свою очередь
                                Console.WriteLine("Player send step ouy of order!");
                            }
                        }
                        else
                        {
                            if (map.NumberPlayerStep == 1)
                            {
                                Map newMap = VerifyAndApplyStep(step, map);
                                if ((Object)newMap != null)
                                {
                                    await Clients.User(room.User1Id.ToString()).SendAsync("ServerSendStep", step);
                                    DB.ChangeRoom(playerId, JsonConverter.ConvertToJSON(newMap));

                                    if (newMap.EndGame)
                                    {
                                        int player1Score = newMap.GetScorePlayer(0);
                                        int player2Score = newMap.GetScorePlayer(1);
                                        //Удаляем комнату и добавляем статистику игры
                                        DB.DeleteRoom(playerId);
                                        DB.AddGameStatistic(10, player1Score, player2Score,
                                            DateTime.Now, GameMode.Default, room.User1Id, room.User2Id);

                                        //Юзер статистика 
                                        UserStatistic player1Stat = DB.GetUserStatistic(room.User1Id);
                                        UserStatistic player2Stat = DB.GetUserStatistic(room.User2Id);

                                        //Меняем рекорд набранных очков
                                        if (player1Stat.MaxScore < player1Score)
                                            DB.ChangeUserStatistic(room.User1Id, AttributeUS.MaxScore, player1Score);
                                        if (player2Stat.MaxScore < player2Score)
                                            DB.ChangeUserStatistic(room.User2Id, AttributeUS.MaxScore, player2Score);

                                        //Если ничья
                                        if (player1Score == player2Score)
                                        {
                                            DB.ChangeUserStatistic(room.User1Id, AttributeUS.Draw, 1);
                                            DB.ChangeUserStatistic(room.User2Id, AttributeUS.Draw, 1);
                                        }
                                        //Если выиграл 1 игрок
                                        else if (player1Score > player2Score)
                                        {
                                            DB.ChangeUserStatistic(room.User1Id, AttributeUS.Win, 1);
                                            DB.ChangeUserStatistic(room.User2Id, AttributeUS.Lose, 1);
                                        }
                                        //Если выиграл 2 игрок
                                        else if (player1Score < player2Score)
                                        {
                                            DB.ChangeUserStatistic(room.User1Id, AttributeUS.Lose, 1);
                                            DB.ChangeUserStatistic(room.User2Id, AttributeUS.Win, 1);
                                        }
                                        // вызвать метод конца игры на клиентах
                                        await Clients.User(room.User1Id.ToString()).SendAsync("ServerEndGame");
                                        await Clients.User(room.User2Id.ToString()).SendAsync("ServerEndGame");
                                    }
                                }
                            }
                            else
                            {
                                //Тут логика, что игрок прислал ход не в свою очередь
                                Console.WriteLine("Player send step ouy of order!");
                            }
                        }
                    }
                    break;

                default:
                    break;
                    
            } 
        });
    }

    //Метод для валидации ходов
    private Map VerifyAndApplyStep(string _step, Map _map)
    {
        Step step = JsonConverter.ConvertJSONtoSTEP(_step);

        if (step.IsReal(_map) == false)
        {
            return null;
        }

        Map newMap = GameStateCalcSystem.ApplyStep(_map, step.Figure, step.Cell);
        return newMap;
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