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
    [SerializeField]
    private AudioController audioController;

    private bool IsFirstGame = true;

    public void StartGame()
    {
        // ������ ���� 
        // ������� ����� ������ � �������

        // ��������� �� ���� �� ���� �� ����� - ���� �� - ������� ������ �����
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
        // ������ �� ������ ����� ������ � ��� ������ - ���� �� ��� �������� ���������

        Cell cell = CurrentGameState.GetCell(cellView.Pos);
        Figure figure = CurrentGameState.GetCell(figureController.UpedFigure.Pos).figure;

        ApplyStepView(cell, figure);
    }

    public void FigureOnClicked(FigureView figureView)
    {
        // �������� ������ �� ������� ������, ������� ��� �� ��� ��������� ����
        // � �������� ��������� (� ��� �� BoxColiders � ������, �� ������� ���������� ���������)

        Figure selectFigure = CurrentGameState.GetCell(figureView.Pos).figure;


        if (selectFigure != null)
        {
            List<Cell> allSteps = WayCalcSystem.CalcAllSteps(CurrentGameState, selectFigure);
            cellController.ShowAllSteps(allSteps);
            cellController.OnBoxColidersForList(allSteps);
        }
        else
        {
            Debug.Log("����� ������ �� �������");
        }
    }

    public void ApplyStepView(Cell cell, Figure figure)
    {
        // ��������� ��� - ���������� �� � Unity
        // �������� ������ ���� - ��������� �������� ������ �� ����� ���� � ������������� �������
        // ����� ������ �� � Model
        // � � ����� ��������� ����� ���

        List<Cell> way = WayCalcSystem.CalcWay(CurrentGameState, figure.pos, cell.pos, figure);

        Map map = GameStateCalcSystem.ApplyStep(CurrentGameState, figure, cell);
        gameStates.Add(map);

        List<Vector3> wayVectors = new List<Vector3>();

        for (int i = 0; i < way.Count; i++)
        {
            wayVectors.Add(new Vector3(way[i].pos.X, 0f, way[i].pos.Y));
        }

        //� ������ ����� ������ -> � ����� ������
        if (cell.figure != null)
        {
            figureController.EatFigureView(cell.figure, CurrentGameState);
        }

        StartCoroutine(figureController.AnimateMoveFigure(figureController.UpedFigure, wayVectors));
        cellController.HideAllPrompts();

        DrawNewGameState();

        StartNewStep();
    }

    public void DrawNewGameState()
    {
        // ��� ����� ����� ��������� �������� ���-�����
        DrawNewGameState(PreviousvGameState);
    }   

    public void DrawNewGameState(Map CompareMap)
    {
        // ���� ��������� ������ � ������ ���������� �� ��������� � ���������� ����������
        // �� ������ � �� ����
        // � ��� �� ������ ���� �� UI-board

        // 0 - ������ �� ���������� 1 - ��������� ����������� ������ 2 - ������� ����������� ������
        int SoundCell = 0;

        for (int i = 0; i < CurrentGameState.Length; i++)
        {
            for (int j = 0; j < CurrentGameState.Width; j++)
            {
                if (CurrentGameState.GetCell(i, j) != CompareMap.GetCell(i, j))
                {
                    cellController.ChangeMaterialCell(i, j, CurrentGameState);

                    // ���� ������ ������������� � Dark
                    if (PreviousvGameState.GetCell(i, j).type != CellType.Dark &&
                        CurrentGameState.GetCell(i, j).type == CellType.Dark)
                    {
                        SoundCell = 1;
                    }

                    // ���� ������ ������������� �� Dark 
                    if (PreviousvGameState.GetCell(i, j).type == CellType.Dark &&
                        CurrentGameState.GetCell(i, j).type != CellType.Dark)
                    {
                        SoundCell = 2;
                    }
                }
            }
        }

        if (SoundCell == 1)
        {
            audioController.PlayAudio(SoundType.DarkCapture);
        }
        if (SoundCell == 2)
        {
            audioController.PlayAudio(SoundType.ReverseDarkCapture);
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
        // ����� ���

        // �������� �� ��, ��� ���� �� �����������
        TestCheckImmutabilityGameState();

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
        //// �������� ��� �� ��������� 4 ���� �� ����� ���� ���-�� ����������

        //if (gameStates.Count <= 4)
        //{
        //    return;
        //}

        //var map1 = gameStates[gameStates.Count - 1];
        //var map2 = gameStates[gameStates.Count - 2];
        //var map3 = gameStates[gameStates.Count - 3];
        //var map4 = gameStates[gameStates.Count - 4];

        //var score1 = map1.score;
        //var score2 = map2.score;
        //var score3 = map3.score;
        //var score4 = map4.score;

        //if ((score1[-1][CellType.Empty] == score2[-1][CellType.Empty]) &&
        //    (score2[-1][CellType.Empty] == score3[-1][CellType.Empty]) &&
        //    (score3[-1][CellType.Empty] == score4[-1][CellType.Empty]))
        //{
        //    if (
        //        (map1.PlayersCount == map2.PlayersCount) &&
        //        (map2.PlayersCount == map3.PlayersCount) &&
        //        (map3.PlayersCount == map4.PlayersCount)
        //        )
        //    {
        //        for (int i = 0; i < map1.PlayersCount; i++)
        //        {
        //            if (
        //                (score1[i][CellType.Paint] == score1[i][CellType.Paint]) &&
        //                (score2[i][CellType.Paint] == score3[i][CellType.Paint]) &&
        //                (score3[i][CellType.Paint] == score4[i][CellType.Paint])
        //                )
        //            {
        //                if (
        //                    (score1[i][CellType.Dark] == score1[i][CellType.Dark]) &&
        //                    (score2[i][CellType.Dark] == score3[i][CellType.Dark]) &&
        //                    (score3[i][CellType.Dark] == score4[i][CellType.Dark])
        //                    )
        //                {
        //                    //
        //                }
        //                else
        //                {
        //                    return;
        //                }
        //            }
        //            else
        //            {
        //                return;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        return;
        //    } 
        //}
        //else
        //{
        //    return;
        //}

        //Debug.Log("����� ����������� 4 ����, ����� ����!");
        //EndGame();
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
        // ����� ����
        Debug.Log("����� ����");
        cameraController.SwitchCameraWithDelay(CameraViewType.noteMenu);

        // ��������� - � �� ����-�� ��� ������� 

        figureController.UpedFigure = null;
        figureController.OFFAllBoxColiders();
        cellController.OFFALLBoxColiders();

        IsFirstGame = false;
        gameStateBuilder = new GameStateBuilder();
        gameStates = new List<Map>();
    }

    public void BackStep()
    {
        // ���������� ��� ����� �� 1
        if (gameStates.Count <= 2) return;

        Map map = CurrentGameState;

        gameStates.RemoveAt(gameStates.Count - 1);

        figureController.DestroyAll();
        figureController.CreateFigures(CurrentGameState);

        DrawNewGameState(map);

        StartNewStep();
    }

    private void LoadMap(Map map)
    {
        gameStates.Add(map);
        DrawNewMap(map);
    }

    private void DrawNewMap(Map map)
    {
        figureController.DestroyAll();
        figureController.CreateFigures(map);

        DrawNewGameState();

        StartNewStep();
    }

    private IEnumerator AIStep()
    {
        yield return new WaitForSeconds(0.1f);

        TestAI.TestMaps = new List<Map>(10000);
        TestAI.TestHash = new Dictionary<int, int>(10000);
        TestAI.TestMAP = new Dictionary<int, Map>(10000);

        TestAI.AlphaBeta(CurrentGameState, 0, int.MinValue, int.MaxValue);

        figureController.UpedFigure = figureController.FindFigure(TestAI.bestFigure, CurrentGameState);

        Debug.Log("���� ���������� �����: " + TestAI.TestCountCalculate);
        TestAI.TestCountCalculate = 0;

        Debug.Log("���������� ����: " + TestAI.TestCountEqualesMap);
        TestAI.TestCountEqualesMap = 0;

        ApplyStepView(TestAI.bestCell, TestAI.bestFigure);
    }

    private void SetFigViewForNewStep()
    {
        // ����������� FigureContoller �� ����� ���
        // �������� � ������ ������� ������ ����� (���� ��� �������) BoxColiders � ��� �����

        figureController.UpedFigure = null;
        figureController.OFFAllBoxColiders();

        if (CurrentGameState.GetPlayerType(CurrentGameState.NumberPlayerStep)  == PlayerType.Human)
        {
            figureController.OnBoxColiders(CurrentGameState.NumberPlayerStep);
        }
    }

    private void SetCellViewForNewStep()
    {
        // ����������� CellConroller �� ����� ���

        cellController.OFFALLBoxColiders();
        cellController.HideAllPrompts();
    }

    public bool GetBoolFigureInCell(Position position)
    {
        return CurrentGameState.GetCell(position).figure != null;
    }
    public Map CurrentGameState { get { return gameStates[gameStates.Count - 1]; } }
    public Map PreviousvGameState { get { return gameStates[gameStates.Count - 2]; } }



    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            testLoad();
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            TestSerialization.Save(CurrentGameState);
        }

    }

    public void testLoad()
    {
        Map loadMap = TestSerialization.Load();

        LoadMap(loadMap);
    }

}
