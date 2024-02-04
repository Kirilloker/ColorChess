using ColorChessModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerSender : MonoBehaviour, IServerSender
{
    MainController mainController;

    private void Awake()
    {
        Print.Log("Awake Server Sender");
        mainController = MainController.Instance;
        Server.Instance.SetServerSender(this);
    }

    public void EndGame()
    {
        UnityMainThreadDispatcher.Instance().Enqueue(() => { mainController.EndGame(); });
    }

    public void SendStep(Step step)
    {
        UnityMainThreadDispatcher.Instance().Enqueue(() => { mainController.ApplyStepView(step); });
    }

    public void StartGame(Map map)
    {
        Print.Log("Start Game Online");
        UnityMainThreadDispatcher.Instance().Enqueue(() => { mainController.StartGame(map); });
    }


}
