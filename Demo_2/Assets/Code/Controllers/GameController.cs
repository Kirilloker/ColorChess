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

    public void SelectGameMode(GameModeType gameMode)
    {
        switch (gameMode)
        {
            case GameModeType.HumanTwo:
                gameStateBuilder.SetDefaultHotSeatGameState();
                break;
            case GameModeType.HumanFour:
                //
                break;
            case GameModeType.AI:
                gameStateBuilder.SetDefaultAIGameState();
                break;
            case GameModeType.Network:
                gameStateBuilder.SetDefaultOnlineGameState();
                break;
            default:
                break;
        }

    }

    public void CellOnClicked(CellView cellView)
    {
        // Нажать на клетку можно только в том случае - если на ней включены подсказки

        // ИСПРАВИТЬ
        ColorChessModel.Cell cell = CurrentGameState.GetCell(cellView.Pos);
        ColorChessModel.Figure figure = CurrentGameState.GetCell(figureController.UpedFigure.Pos).figure;

        ApplyStepView(cell, figure);
    }

    public void ApplyStepView(ColorChessModel.Cell cell, ColorChessModel.Figure figure)
    {
        // Применяем ход - отображаем всё в Unity
        // Получаем массив пути - запускаем анимацию фигуры по этому пути и перекрашиваем клеткти
        // Также меняем всё в Model
        // И в конце запускаем новый шаг



        List<ColorChessModel.Cell> way = WayCalcSystem.CalcWay(CurrentGameState, figure.pos, cell.pos, figure);

        Map map = GameStateCalcSystem.ApplyStep(CurrentGameState, figure, cell);
        gameStates.Add(map);

        List<Vector3> wayVectors = new List<Vector3>();

        for (int i = 0; i < way.Count; i++)
        {
            wayVectors.Add(new Vector3(way[i].pos.X, 0f, way[i].pos.Y));
        }

        //В клетке стоит фигура -> её хотят съесть
        if (cell.figure != null)
        {
            figureController.EatFigureView(cell.figure, CurrentGameState);
        }

        figureController.AnimateMoveFigure(figureController.UpedFigure, wayVectors);
        cellController.HideAllPrompts();

        DrawNewGameState(CurrentGameState);

        StartNewStep();
    }

    public void DrawNewGameState(Map gameState)
    {
        // Если состояние клетки в модели изменилось по сравнению с предыдущим состоянием
        // То меняем у неё цвет

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
        // Получаем фигуру по которой нажали, считаем для неё все возможные пути
        // И включаем подсказки (А так же BoxColiders у клеток, на которых включились подсказки)

        // ИСПРАВИТЬ
        ColorChessModel.Figure selectFigure = CurrentGameState.GetCell(figureView.Pos).figure;


        if (selectFigure != null)
        {
            // ИСПРАВИТЬ
            List<ColorChessModel.Cell> allSteps = WayCalcSystem.CalcAllSteps(CurrentGameState, selectFigure);
            cellController.ShowAllSteps(allSteps);
            cellController.OnBoxColidersForList(allSteps);
        }
        else
        {
            Debug.Log("Такой фигуры не нашлось");
        }
    }

    public void StartGame()
    {
        // Начало игры 
        // Создаем Доску Клетки и Игроков
        TestCheckNewGame();

        gameStates.Add(gameStateBuilder.CreateGameState());

        boardController.CreateBoard(CurrentGameState);
        cellController.CreateCells(CurrentGameState);
        figureController.CreateFigures(CurrentGameState);

        cameraController.SwitchCameraWithDelay(CameraViewType.inGame1);

        StartNewStep();
    }

    private void SetFigViewForNewStep()
    {
        // Настраиваем FigureContoller на новый ход
        // Включаем у игрока который сейчас ходит (если это человек) BoxColiders у его фигур
        
        figureController.UpedFigure = null;
        figureController.OFFAllBoxColiders();
        
        if (CurrentGameState.players[CurrentGameState.numberPlayerStep].type == PlayerType.Human) 
        {
            figureController.OnBoxColiders(CurrentGameState.numberPlayerStep);
        }
    }
    
    private void SetCellViewForNewStep()
    {
        // Настраиваем CellConroller на новый ход

        cellController.OFFALLBoxColiders();
    }

    public void StartNewStep()
    {
        // Новый ход

        if (CurrentGameState.EndGame == true)
        {
            EndGame();
            return;
        }

        
        PlayerType playerType = CurrentGameState.players[CurrentGameState.numberPlayerStep].type;

        switch (playerType)
        {
            case PlayerType.Human:
                SetFigViewForNewStep();
                SetCellViewForNewStep();

                break;

            case PlayerType.AI:
                StartCoroutine(TestAIStep());
                //TestAIStep();
                break;
            case PlayerType.Online:
                //
                //
                break;
        }


        if (playerType == PlayerType.Human)
        {

        }

    }

    private void TestCheckNewGame()
    {
        if (gameStates.Count != 0)
        {
            gameStates = new List<Map>();
            DestroyAll();
        }
    }

    public void DestroyAll()
    {
        figureController.DestroyAll();
        cellController.DestroyAll();
        boardController.Destroy();

        boardController.ShowBoardDecor();
    }

    public void EndGame()
    {
        // Конец игры

        Debug.Log("Игра закончилась");
        cameraController.SwitchCameraWithDelay(CameraViewType.noteMenu);
    }
    public Map CurrentGameState { get { return gameStates[gameStates.Count-1]; } }
    public Map PreviousvGameState { get { return gameStates[gameStates.Count - 2]; } }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        EndGame();
    //    }
    //}


    private IEnumerator TestAIStep()
    //private void TestAIStep()
    {
        yield return new WaitForSeconds(1f);

        TestAI.TestInt = 0;
        TestAI.maps = new Dictionary<uint, int>(100000);
        TestAI.mapsTest = new Dictionary<uint, Map>(100000);

        TestAI.AlphaBeta(CurrentGameState, 0, int.MinValue, int.MaxValue);

        figureController.UpedFigure = figureController.FindFigure(TestAI.bestFigure1, CurrentGameState);

        ApplyStepView(TestAI.bestCell1, TestAI.bestFigure1);
    }
}
