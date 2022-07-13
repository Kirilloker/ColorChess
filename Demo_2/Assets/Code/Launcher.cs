using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;


public class Launcher : MonoBehaviourPunCallbacks
{
    private const byte maxPlayers = 2;
    private ExitGames.Client.Photon.Hashtable playerPropriets = new ExitGames.Client.Photon.Hashtable();
    
    //»вент заполненной комнаты
    private const byte RoomIsFullEventCode = 1;
    private const byte StepEventCode = 2;

    public void SendMyStep( string _name_figure, Vector3 _position_step)
    {
        object[] stepInfo = new object[] { _name_figure, _position_step, PhotonNetwork.LocalPlayer.UserId };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(StepEventCode, stepInfo, raiseEventOptions, SendOptions.SendReliable);
    }

    public void SendRoomIsFull()
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(RoomIsFullEventCode, null, raiseEventOptions, SendOptions.SendReliable);
    }

    /*
    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;

        if (eventCode == RoomIsFullEventCode)
        {
            Log("Room is full, my player list: ");
            Log(PhotonNetwork.PlayerList.ToStringFull());
            Log("\nThem data: ");

            for (int i = 0; i < PhotonNetwork.CurrentRoom.MaxPlayers; i++)
            {
                Log(PhotonNetwork.PlayerList[i].CustomProperties["Data"].ToString());
                Log(PhotonNetwork.PlayerList[i].CustomProperties["Figures"].ToString());
            }
        }
    }
    */

    public void Connect()
    {
        PhotonNetwork.GameVersion = "v 0.1";
        PhotonNetwork.NickName = "Player" + Random.Range(1, 100);
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("PRESSED BUTTON Connect");
    }

    //ѕереопредел€ю обратные вызовы_______________________________________
    public override void OnConnectedToMaster()
    {
        Log("I connected to master server, try to join the room\n");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {
        Log("I joined the room\n");

        PhotonNetwork.SetPlayerCustomProperties(playerPropriets);

        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            Log("Start event\n");
            SendRoomIsFull();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Log("Join failed, try to create room(" + message + ")\n");
        PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions { MaxPlayers = maxPlayers, PublishUserId = true, IsVisible = true });
    }

    public override void OnCreatedRoom()
    {
        Log("I created the room\n");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Log("I cant create the room(" + message + ")\n");
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Log("Another player connected! (" + newPlayer.NickName + ")\n");
        Log("Current num of players = " + PhotonNetwork.CurrentRoom.PlayerCount.ToString() + "\n");
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        Log("Another player disconnected! (" + otherPlayer.NickName + ")\n");
    }

    public void Log(string messege)
    {
        //text.text += messege;
        Debug.Log(messege);
    }



    public void SetPlayerPrefs(string nickname, Corner corner, Color color)
    {
        Debug.Log("PRESSED BUTTON SetPlayerPrefs2");
        playerPropriets.Add("nickname", nickname);
        playerPropriets.Add("corner", corner);
        playerPropriets.Add("color", color);
    }

    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        Log(targetPlayer.NickName);
        Log(changedProps.ToStringFull());
    }
}

