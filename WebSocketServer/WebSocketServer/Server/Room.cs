using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColorChessModel;

public class Room
{
    //При дальнейшей разработке возможно стоит переписать класс под n игроков в комнате
    private string player1ID;
    private string player2ID;
    private DefaultGame connectionsHub;
    private Map? map;
    private GameStateBuilder gameStateBuilder;
    public Room(string player1ID, string player2ID, DefaultGame connectionsHub)
    {
        this.player1ID = player1ID;
        this.player2ID = player2ID;
        this.connectionsHub = connectionsHub;
        gameStateBuilder = new GameStateBuilder();
    }

    public async void StartGame()
    {
        await Task.Run(() =>
        {
            //Создаем игровое состояние
            gameStateBuilder.SetDefaultHotSeatGameState();
            map = gameStateBuilder.CreateGameState();

            //Формируем сообщения и отправляем игрокам
            string msgToPlayer1 = "1" + JsonConverter.ConvertToJSON(map.ConvertMapToPlayer(0));
            string msgToPlayer2 = "1" + JsonConverter.ConvertToJSON(map.ConvertMapToPlayer(1));
            connectionsHub.SendMessageTo(msgToPlayer1, player1ID);
            connectionsHub.SendMessageTo(msgToPlayer2, player2ID);
        });
    }

    public async void ApplyPlayerStep(string playerID, string step)
    {
        await Task.Run(() =>
        {
            if(playerID == player1ID)
            {
                if (map.NumberPlayerStep == 0)
                {
                    Map newMap = VerifyAndApplyStep(step, map);
                    if((Object)newMap != null)
                    {
                        map = newMap;
                        connectionsHub.SendMessageTo("2" + step, player2ID);
                        if (newMap.EndGame) CloseRoom();
                    }
                    else
                    {
                        //Тут выполняется логикка если ход не прошел верификацию
                    }
                }
            }
            else if (playerID == player2ID)
            {
                if (map.NumberPlayerStep == 1)
                {
                    Map newMap = VerifyAndApplyStep(step, map);
                    if ((Object)newMap != null)
                    {
                        map = newMap;
                        connectionsHub.SendMessageTo("2" + step, player1ID);
                        if (newMap.EndGame) CloseRoom();
                    }
                    else
                    {
                        //Тут выполняется логикка если ход не прошел верификацию
                    }
                }
            }
           
        });
    }

    private  Map VerifyAndApplyStep(string _step, Map _map)
    {
        Step step = JsonConverter.ConvertJSONtoSTEP(_step);

        if (step.IsReal(_map) == false)
        {
            return null;
        }

        Map newMap = GameStateCalcSystem.ApplyStep(_map, step.Figure, step.Cell);
        return newMap;
    }


    public async void CloseRoom()
    {
        await Task.Run(() =>
        {
            DefaultGameRoomManager.DeleteRoom(player1ID, player2ID);
            
        });
    }
}