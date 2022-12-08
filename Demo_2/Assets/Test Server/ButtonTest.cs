using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTest : MonoBehaviour
{
    private SignalRClient signalRClient;

    void Start()
    {
        signalRClient = GameObject.Find("SignalRClient").GetComponent<SignalRClient>();
    }

    public void SearchOpponent()
    {
        signalRClient.SearchOpponent();
    }

    public void ConnectServer()
    {
        signalRClient.Connect();
    }
}
