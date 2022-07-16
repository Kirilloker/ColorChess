using ColorChessModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameController gameController;

    public void SelectStandartHotSeat()
    {
        gameController.SelectGameMode(GameModeType.HumanTwo);
    }

    public void SelectStandartAI()
    {
        gameController.SelectGameMode(GameModeType.AI);
    }

    public void SelectStandartNetwork()
    {
        gameController.SelectGameMode(GameModeType.Network);
    }

    public void StartGame()
    {
        gameController.StartGame();
    }
}
