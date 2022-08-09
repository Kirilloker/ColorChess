using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

enum MessageType
{
    GameSearch = 0,
    CurrentGameState = 1,
    PlayerStep = 2,
    InvalidStep = 3
}

public class Server : MonoBehaviour
{
    private const string DefaultGameServerUrl = "ws://127.0.0.1:7890/DefaultGame";
    private WebSocket ws;

    void Start()
    {
        ConnectToDefaultGame();
    }
    
    public void ConnectToDefaultGame()
    {
       OpenConnection(DefaultGameServerUrl);
        ws.Send("0WannaPlay");
    }

    public void OpenConnection(string url)
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

    private void Ws_OnMessage(object sender, MessageEventArgs e)
    {
        MessageType msgType = (MessageType)int.Parse(e.Data.ToString().Substring(0, 1));
        string msg = e.Data.ToString().Substring(1);

        switch (msgType)
        {
            case MessageType.CurrentGameState:
                Debug.Log($"I recieved game state: {msg}");
                break;
            case MessageType.InvalidStep:
                break;
            default:
                break;
        }
    }

    private void Ws_OnError(object sender, ErrorEventArgs e)
    {
        Debug.Log(e.Message);
    }

    private void Ws_OnClose(object sender, CloseEventArgs e)
    {
        Debug.Log(ws.ReadyState);
    }

    private void Ws_OnOpen(object sender, System.EventArgs e)
    {
        Debug.Log(ws.ReadyState);
    }
}
