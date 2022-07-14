using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameController gameController;

    public void SelectStandartHotSeat()
    {
        gameController.SelectGameMode("HotSeat");
    }

    public void SelectStandartAI()
    {
        gameController.SelectGameMode("AI");
    }

    public void SelectStandartNetwork()
    {
        gameController.SelectGameMode("Network");
    }

    public void StartGame()
    {
        gameController.StartGame();
    }
}
