using ColorChessModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using System.Threading.Tasks;

enum MessageType
{
    GameSearch = 0,
    CurrentGameState = 1,
    PlayerStep = 2,
    InvalidStep = 3
}

public class Server : MonoBehaviour
{
    //private const string DefaultGameServerUrl = "ws://127.0.0.1:7890/DefaultGame";
    private const string DefaultGameServerUrl = "ws://192.168.0.42:7890/DefaultGame";
    private WebSocket ws;
   
    
    public void ConnectToDefaultGame()
    {
       OpenConnection(DefaultGameServerUrl);
        ws.Send("0WannaPlay");
    }

    private void OpenConnection(string url)
    {
        if (ws != null)
        {
            CloseConnection();
        }

        ws = new WebSocket(url);

        ws.OnOpen += Ws_OnOpen;
        ws.OnClose += Ws_OnClose;
        ws.OnMessage += Ws_OnMessage;
        ws.OnError += Ws_OnError;

        ws.Connect();
    }

    public void CloseConnection()
    {
        ws.Close();
        ws = null;
    }

    //Puck-Puck
    public GameController gameController;
    private void Ws_OnMessage(object sender, MessageEventArgs e)
    {

        MessageType msgType = (MessageType)int.Parse(e.Data.ToString().Substring(0, 1));
        string msg = e.Data.ToString().Substring(1);

        switch (msgType)
        {
            case MessageType.CurrentGameState:
                Debug.Log($"I recieved game state: {msg}");
                Map map = TestServerHelper.ConvertJSONtoMap(msg);
                StartGame(map);
                break;
            case MessageType.InvalidStep:
                break;
            case MessageType.PlayerStep:
                Debug.Log($"I recieved game state: {msg}");
                ApplyPlayerStep(TestServerHelper.ConvertJSONtoSTEP(msg));
                break;
            default:
                break;
        }
    }

    private void Ws_OnError(object sender, ErrorEventArgs e)
    {
        Debug.Log(e.Message);
        Debug.Log(e.Exception.ToString());
    }

    private void Ws_OnClose(object sender, CloseEventArgs e)
    {
        Debug.Log(ws.ReadyState);
    }

    private void Ws_OnOpen(object sender, System.EventArgs e)
    {
        Debug.Log(ws.ReadyState);
    }

    private async void StartGame(Map map)
    {
       await Task.Run(() => { gameController.StartGame(map); });
    }

    private async void ApplyPlayerStep(Step step)
    {
        await Task.Run(() => { gameController.ApplyStepView(step); });
    }

    public void SendStep(Step clientStep)
    {
        ws.Send("2" + TestServerHelper.ConvertToJSON(clientStep));
    }
}
