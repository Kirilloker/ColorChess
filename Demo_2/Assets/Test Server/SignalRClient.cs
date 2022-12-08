using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using BestHTTP.SignalR;
using BestHTTP.SignalR.Hubs;
using BestHTTP.SignalR.Messages;

public class SignalRClient : MonoBehaviour 
{
    // Declaration the variables

    // SignalR variables
    //private static Uri uri = new Uri("http://192.168.1.38:11000/Chat");
    private static Uri uri = new Uri("http://localhost:5000/signalr/");
    private static GameHub gameHub;
    private static Connection signalRConnection;

    // Other variables
    private SignalRClient signalRClient;
    private float searchTimeOut = -1;

    public static string playerName;

    


    public void Connect()
    {
        signalRClient = this;

        // Initialize the connection
        gameHub = new GameHub(ref signalRClient);
        signalRConnection = new Connection(uri, gameHub);
        signalRConnection.Open();
        
        signalRConnection.OnConnected += (conn) => 
        {
            Debug.Log("Connect Successfully!");
        };
        signalRConnection.OnError += (conn, err) =>
        {
            Debug.Log(err);
        };
    }

    public void SearchOpponent()
    {
        // Call the SearchOpponent function from the server and set a timeout to 60 seconds
        signalRConnection[gameHub.Name].Call("SearchOpponent");
        searchTimeOut = Time.time + 60;
    }

    public void SendChat(string message)
    {
        signalRConnection[gameHub.Name].Call("SendChat", message);
    }


	void Start () 
    {
        DontDestroyOnLoad(this);
	}
	
	void FixedUpdate () 
    {
        // If search timeout
        if (searchTimeOut != -1 && Time.time > searchTimeOut)
        {
            signalRConnection.Close();
            Debug.Log("No oppenend found");

            // Set searchTimeOut to -1
            searchTimeOut = -1;
        }
	}

    void OnApplicationQuit()
    {
        if(signalRConnection.State != ConnectionStates.Closed)
            signalRConnection.Close();
    }

    public class GameHub : Hub
    {
        private SignalRClient signalRClient;

        public GameHub(ref SignalRClient signalRClient) : base("GameHub")
        {
            this.signalRClient = signalRClient;

            // Register callback functions that received from the server
            base.On("JoinToOpponent", Joined);
            base.On("OpponentLeft", Left);
            base.On("OpponentMessage", Message);
        }

        private void Joined(Hub hub, MethodCallMessage msg)
        {
            Debug.Log("Joined");
            Debug.Log(msg.Arguments[0].ToString());
            int playerId = int.Parse(msg.Arguments[0].ToString());

            if (playerId == -1)
            {
                Debug.Log("Waiting opponent");
            }
            else
            {
                // Set searchTimeOut to -1
                signalRClient.searchTimeOut = -1;

                // Set your GameObject name and start the game 
                playerName = "Player" + playerId;
                Debug.Log("Ура победа вы нашли друга");

                GameController gameController = GameObject.Find("GameController").GetComponent<GameController>();
                gameController.SelectGameMode(ColorChessModel.GameModeType.Network);
                gameController.StartGame();
            }
        }


        private void Left(Hub hub, MethodCallMessage msg)
        {
            Debug.Log("Player Disconnected!");
        }


        private void Message(Hub hub, MethodCallMessage msg)
        {
            string message = msg.Arguments[0].ToString();

            Debug.Log(message);
            GameController gameController = GameObject.Find("GameController").GetComponent<GameController>();
            gameController.ApplyStepView(TestServerHelper.ConvertJSONtoSTEP(message));
            Debug.Log("Message");
        }
    }
}
