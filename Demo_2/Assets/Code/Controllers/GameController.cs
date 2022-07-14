using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ColorChessModel;

public class GameController : MonoBehaviour
{
    private List<Map> gameStates = new List<Map>();

    private GameStateBuilder gameStateBuilder = new GameStateBuilder();

    public UIController uiController;
    public FigureController figureController;
    public CellController cellController;
    public CameraController cameraController;

    public void SelectGameMode(string str)
    {
        switch (str)
        {
            case "HotSeat":
                gameStateBuilder.SetDefaultHotSeatGameState();
                break;
            case "AI":
                gameStateBuilder.SetDefaultAIGameState();
                break;
            case "Network":
                gameStateBuilder.SetDefaultOnlineGameState();
                break;
            default:
                break;
        }
    }

    public void StartGame()
    {
        gameStates.Add(gameStateBuilder.CreateGameState());

        cellController.Spawn(CurrentGameState);
        figureController.Spawn(CurrentGameState);

        //cameraController.
    }



    public Map CurrentGameState { get { return gameStates[gameStates.Count-1]; } }
}
