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

    public void CellOnClicked(CellView cellView)
    {
        // ÈÑÏÐÀÂÈÒÜ
        ColorChessModel.Cell cell = CurrentGameState.GetCell(cellView.Pos);
        ColorChessModel.Figure figure = CurrentGameState.GetCell(figureController.UpedFigure.Pos).figure;

        List<ColorChessModel.Cell> way = WayCalcSystem.CalcWay(CurrentGameState, figure.pos, cell.pos, figure);

        Map map = GameStateCalcSystem.ApplyStep(CurrentGameState, figure, cell);
        gameStates.Add(map);

        List<Vector3> wayVectors = new List<Vector3>();

        for (int i = 0; i < way.Count; i++)
        {
            wayVectors.Add(new Vector3(way[i].pos.X, 0f, way[i].pos.Y));
        }

        figureController.AnimateMoveFigure(figureController.UpedFigure, wayVectors);
        cellController.HideAllPrompts();

        DrawNewGameState(CurrentGameState);

        StartNewStep();
    }

    public void DrawNewGameState(Map gameState)
    {
        for (int i = 0; i < CurrentGameState.Length; i++)
        {
            for (int j = 0; j < CurrentGameState.Width; j++)
            {
                if (CurrentGameState.cells[i, j] != PreviousvGameState.cells[i, j])
                {
                    cellController.ChangeMaterialCell(i, j, CurrentGameState);
                }
            }
        }
    }

    public void FigureOnClicked(FigureView figureView)
    {
        // ÈÑÏÐÀÂÈÒÜ
        ColorChessModel.Figure selectFigure = CurrentGameState.GetCell(figureView.Pos).figure;


        if (selectFigure != null)
        {
            // ÈÑÏÐÀÂÈÒÜ
            List<ColorChessModel.Cell> allSteps = WayCalcSystem.CalcAllSteps(CurrentGameState, selectFigure);
            cellController.ShowAllSteps(allSteps);
            cellController.OnBoxColidersForList(allSteps);
        }
        else
        {
            Debug.Log("Òàêîé ôèãóðû íå íàøëîñü");
        }
    }

    public void StartGame()
    {
        gameStates.Add(gameStateBuilder.CreateGameState());

        boardController.CreateBoard(CurrentGameState);
        cellController.CreateCells(CurrentGameState);
        figureController.CreateFigures(CurrentGameState);

        //cameraController.

        StartNewStep();
    }

    private void SetFigViewForNewStep()
    {
        figureController.UpedFigure = null;
        figureController.OFFAllBoxColiders();
        

        if (CurrentGameState.players[CurrentGameState.numberPlayerStep].type == PlayerType.Human) 
        {
            figureController.OnBoxColiders(CurrentGameState.numberPlayerStep);
        }
        
    }
    
    private void SetCellViewForNewStep()
    {
        cellController.OFFALLBoxColiders();
    }

    public void StartNewStep()
    {
        if (CurrentGameState.EndGame == true)
        {
            Debug.Log("Èãðà çàêîí÷èëàñü");
            return;
        }


        SetFigViewForNewStep();
        SetCellViewForNewStep();

    }
    public Map CurrentGameState { get { return gameStates[gameStates.Count-1]; } }
    public Map PreviousvGameState { get { return gameStates[gameStates.Count - 2]; } }
}
