using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;


public class test_server : MonoBehaviourPunCallbacks, IOnEventCallback
{
    public int ScaleBoard = 9;

    Corner corner_player_1 = Corner.Down_left;
    Corner corner_player_2 = Corner.Up_right;

    Color color_player_1 = Color.Red;
    Color color_player_2 = Color.Blue;

    string nickname_player_1 = "kirillok";
    string nickname_player_2 = "Morgenshtern";

    List<Player_description> players_discription = new List<Player_description>();

    Play_session_settings _pl;    

    Dictionary<Vector3, string> f1  = new Dictionary<Vector3, string>
    {
        { new Vector3(0f, 0f, 3f), "Pawn"},
        { new Vector3(1f, 0f, 2f), "Pawn"},
        { new Vector3(2f, 0f, 1f), "Pawn"},
        { new Vector3(3f, 0f, 0f), "Pawn"},
        { new Vector3(0f, 0f, 2f), "Castle"},
        { new Vector3(0f, 0f, 1f), "Queen"},
        { new Vector3(1f, 0f, 1f), "Horse"},
        { new Vector3(1f, 0f, 0f), "King"},
        { new Vector3(2f, 0f, 0f), "Bishop"},
    }; 

    private void Awake()
    {
        _pl = GameObject.FindWithTag("play_session_setting").GetComponent<Play_session_settings>();
    }

    private void OnMouseUp()
    {
        create_play_session();
    }

    public void create_play_session()
    {
        Debug.Log("Create play session");

        int width = ScaleBoard;
        int lenght = ScaleBoard;

        players_discription = new List<Player_description>();

        _pl.set_lenght(lenght);
        _pl.set_width(width);
        _pl.set_camera_swap(false);

        connect_server();
    }

    void connect_server()
    {
        Launcher _launcer = this.GetComponent<Launcher>();

        //_launcer.SetPlayerPrefs(nickname_player_1, corner_player_1, color_player_1);
        _launcer.SetPlayerPrefs(nickname_player_2, corner_player_2, color_player_2);
        _launcer.Connect();

    }

    int online_number = 0;
    void start_game()
    {
        _pl.set_online_number(online_number);
        _pl.set_players_discription(players_discription);
        _pl.create_play_session();
    }


    private const byte RoomIsFullEventCode = 1;
    private const byte StepEventCode = 2;
    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;

        if (eventCode == RoomIsFullEventCode)
        {
            Log("Room is full, my player list: ");
            Log(PhotonNetwork.PlayerList.ToString());


            for (int i = 0; i < PhotonNetwork.CurrentRoom.MaxPlayers; i++)
            {

                if (PhotonNetwork.PlayerList[i].UserId == PhotonNetwork.LocalPlayer.UserId)
                {
                    online_number = i + 1;
                    Player_description pd2 = new Player_description(f1, (Color)PhotonNetwork.PlayerList[i].CustomProperties["color"], "Network", i + 1, PhotonNetwork.PlayerList[i].CustomProperties["nickname"].ToString(), (Corner)PhotonNetwork.PlayerList[i].CustomProperties["corner"]);
                    players_discription.Add(pd2);
                }
                else 
                {
                    Player_description pd2 = new Player_description(f1, (Color)PhotonNetwork.PlayerList[i].CustomProperties["color"], "Network", i + 1, PhotonNetwork.PlayerList[i].CustomProperties["nickname"].ToString(), (Corner)PhotonNetwork.PlayerList[i].CustomProperties["corner"]);
                    players_discription.Add(pd2);
                }

                Log(PhotonNetwork.PlayerList[i].CustomProperties["nickname"].ToString());
            }

            start_game();
        }

        if (eventCode == StepEventCode) 
        {
            Debug.Log("StepEvent");
            object[] data = (object[])photonEvent.CustomData;

            if (PhotonNetwork.LocalPlayer.UserId != data[2].ToString())
            {
                Figure _figure = GameObject.Find((string)data[0]).GetComponent<Figure>();
                Vector3 pos = (Vector3)data[1];

               
                Cell _cell = GameObject.Find("Board").GetComponent<Board>().get_cell(pos.x, pos.z);
                
                GameObject.Find("play_session").GetComponent<Play_session>().fake_step(_figure, _cell);
            }
            else 
            {
                Debug.Log("Это я и сходил, так что делать ничего не нужно!!!");
            }
        }
    }



    public void Log(string messege)
    {
        Debug.Log(messege);
    }
}
