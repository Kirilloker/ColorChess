using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ColorChessModel;

public class GameController : MonoBehaviour
{
    private List<Map> gameStates = new List<Map>();

    private GameStateBuilder gameStateBuilder = new GameStateBuilder();

    [SerializeField]
    private UIController uiController;
    [SerializeField]
    private FigureController figureController;
    [SerializeField]
    private CellController cellController;
    [SerializeField]
    private CameraController cameraController;
    [SerializeField]
    private BoardController boardController;

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

        boardController.CreateBoard(CurrentGameState);
        cellController.CreateCells(CurrentGameState);
        figureController.CreateFigures(CurrentGameState);

        //cameraController.
    }



    public Map CurrentGameState { get { return gameStates[gameStates.Count-1]; } }
}
