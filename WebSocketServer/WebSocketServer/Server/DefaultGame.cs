using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

enum MessageType
{
    GameSearch = 0,
    CurrentGameState = 1,
    PlayerStep = 2,
    InvalidStep = 3
}

public class DefaultGame : WebSocketBehavior
{
    private bool flag = true;
    protected override void OnMessage(MessageEventArgs e)
    {
        string msg = e.Data.ToString();
        MessageType msgType = (MessageType)int.Parse(msg.Substring(0, 1));
        msg = msg.Substring(1);
        
        switch (msgType)
        {
            case MessageType.GameSearch:
                DefaultGameRoomManager.FindRoom(ID);
                break;

            case MessageType.PlayerStep:
                DefaultGameRoomManager.SendPlayerStepToRoom(ID, msg);
                break;
            default:
                break;
        }
    }

    protected override void OnOpen()
    {
        if(flag)
        {
            DefaultGameRoomManager.SetConnectionsHub(this);
            flag = false;
        }

        Console.WriteLine($"Player connected(id:{ID})");
        base.OnOpen();
    }

    protected override void OnClose(CloseEventArgs e)
    {
        Console.WriteLine($"Player disconnected(id:{ID})");
        base.OnClose(e);
    }

    protected override void OnError(WebSocketSharp.ErrorEventArgs e)
    {
        Console.WriteLine(e.Message);
        base.OnError(e);
    }

    public async void SendMessageTo(string msg, string ID)
    {
        await Task.Run(()=>Sessions.SendTo(msg, ID));
    }
}
