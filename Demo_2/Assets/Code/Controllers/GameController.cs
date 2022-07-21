using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ColorChessModel;
using System.Threading;
using System;

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

    private bool IsFirstGame = true;

    public void StartGame()
    {
        // Начало игры 
        // Создаем Доску Клетки и Игроков

        // Проверяем не было ли игры до этого - если да - удаляем старую доску
        TestCheckNewGame();

        gameStates.Add(gameStateBuilder.CreateGameState());

        boardController.CreateBoard(CurrentGameState);
        cellController.CreateCells(CurrentGameState);
        figureController.CreateFigures(CurrentGameState);

        cameraController.SwitchCameraWithDelay(CameraViewType.inGame1);

        StartNewStep();
    }

    public void CellOnClicked(CellView cellView)
    {
        // Нажать на клетку можно только в том случае - если на ней включены подсказки

        // ИСПРАВИТЬ
        ColorChessModel.Cell cell = CurrentGameState.GetCell(cellView.Pos);
        ColorChessModel.Figure figure = CurrentGameState.GetCell(figureController.UpedFigure.Pos).figure;

        ApplyStepView(cell, figure);
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
        // А так же меняем Очки на UI-board

        for (int i = 0; i < CurrentGameState.Length; i++)
        {
            for (int j = 0; j < CurrentGameState.Width; j++)
            {
                if (CurrentGameState.GetCell(i, j) != PreviousvGameState.GetCell(i, j))
                {
                    cellController.ChangeMaterialCell(i, j, CurrentGameState);
                }
            }
        }

        boardController.SetScoreUI(CurrentGameState);
    }

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

    public void StartNewStep()
    {
        // Новый ход

        // Проверка на то, что игра не зациклилась
        TestCheckImmutabilityGameState();

        TestSerialization.Save(CurrentGameState);

        if (CurrentGameState.EndGame == true)
        {
            EndGame();
            return;
        }

        PlayerType playerType = CurrentGameState.GetPlayerType(CurrentGameState.NumberPlayerStep);

        switch (playerType)
        {
            case PlayerType.Human:
                SetFigViewForNewStep();
                SetCellViewForNewStep();
                break;

            case PlayerType.AI:
                StartCoroutine(AIStep());
                break;

            case PlayerType.Online:
                //
                //
                break;
        }
    }
    
    private void TestCheckNewGame()
    {
        if (IsFirstGame == false)
        {
            DestroyAll();
        }
    }

    private void TestCheckImmutabilityGameState()
    {
        // Проверка что за последние 4 хода на карте хоть что-то изменилось

        if (gameStates.Count <= 4)
        {
            return;
        }

        var map1 = gameStates[gameStates.Count - 1];
        var map2 = gameStates[gameStates.Count - 2];
        var map3 = gameStates[gameStates.Count - 3];
        var map4 = gameStates[gameStates.Count - 4];

        var score1 = map1.score;
        var score2 = map2.score;
        var score3 = map3.score;
        var score4 = map4.score;

        if ((score1[-1][CellType.Empty] == score2[-1][CellType.Empty]) &&
            (score2[-1][CellType.Empty] == score3[-1][CellType.Empty]) &&
            (score3[-1][CellType.Empty] == score4[-1][CellType.Empty]))
        {
            if (
                (map1.PlayersCount == map2.PlayersCount) &&
                (map2.PlayersCount == map3.PlayersCount) &&
                (map3.PlayersCount == map4.PlayersCount)
                )
            {
                for (int i = 0; i < map1.PlayersCount; i++)
                {
                    if (
                        (score1[i][CellType.Paint] == score1[i][CellType.Paint]) &&
                        (score2[i][CellType.Paint] == score3[i][CellType.Paint]) &&
                        (score3[i][CellType.Paint] == score4[i][CellType.Paint])
                        )
                    {
                        if (
                            (score1[i][CellType.Dark] == score1[i][CellType.Dark]) &&
                            (score2[i][CellType.Dark] == score3[i][CellType.Dark]) &&
                            (score3[i][CellType.Dark] == score4[i][CellType.Dark])
                            )
                        {
                            //
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        return;
                    }
                }
            }
            else
            {
                return;
            } 
        }
        else
        {
            return;
        }

        Debug.Log("Карта повторилась 4 раза, конец игры!");
        EndGame();
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
        Debug.Log("Конец игры");
        cameraController.SwitchCameraWithDelay(CameraViewType.noteMenu);

        // ИСПРАВИТЬ - я бы куда-то это перенес 
        
        IsFirstGame = false;
        gameStateBuilder = new GameStateBuilder();
        gameStates = new List<Map>();
    }

    private IEnumerator AIStep()
    {
        yield return new WaitForSeconds(0.5f);

        //testThread = new Thread(new ThreadStart(AIStepTest));
        //testThread.Start();

        TestAI.AlphaBeta(CurrentGameState, 0, int.MinValue, int.MaxValue);

        figureController.UpedFigure = figureController.FindFigure(TestAI.bestFigure, CurrentGameState);

        ApplyStepView(TestAI.bestCell, TestAI.bestFigure);

    }

    private void SetFigViewForNewStep()
    {
        // Настраиваем FigureContoller на новый ход
        // Включаем у игрока который сейчас ходит (если это человек) BoxColiders у его фигур

        figureController.UpedFigure = null;
        figureController.OFFAllBoxColiders();

        if (CurrentGameState.GetPlayerType(CurrentGameState.NumberPlayerStep)  == PlayerType.Human)
        {
            figureController.OnBoxColiders(CurrentGameState.NumberPlayerStep);
        }
    }

    private void SetCellViewForNewStep()
    {
        // Настраиваем CellConroller на новый ход

        cellController.OFFALLBoxColiders();
    }

    public bool GetBoolFigureInCell(Position position)
    {
        return CurrentGameState.GetCell(position).figure != null;
    }
    public Map CurrentGameState { get { return gameStates[gameStates.Count - 1]; } }
    public Map PreviousvGameState { get { return gameStates[gameStates.Count - 2]; } }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TestSerialization.Load();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            TestSerialization.Save(CurrentGameState);
        }

    }

    public void testLoad()
    {
        Map loadMap = TestSerialization.Load();

        // Тут нужно всё с FigureContoller синхронизировать

        DrawNewGameState(loadMap);
    }

}
