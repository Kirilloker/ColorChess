using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ColorChessModel;
using System.Threading;
using System;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

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
    [SerializeField]
    private Server server;

    private bool IsFirstGame = true;

    private float cameraSpeed;

    

    public void StartGame()
    {
        StartGame(gameStateBuilder.CreateGameState());
    }
    public void StartGame(Map map)
    {
        // ������ ���� 
        // ������� ����� ������ � �������

        // ��������� �� ���� �� ���� �� ����� - ���� �� - ������� ������ �����
        TestCheckNewGame();

        gameStates.Add(map);

        boardController.CreateBoard(CurrentGameState);
        cellController.CreateCells(CurrentGameState);
        figureController.CreateFigures(CurrentGameState);

        cameraController.SwitchCamera(CameraViewType.inGame1);

        StartNewStep();
    }

    public void CellOnClicked(CellView cellView)
    {
        // ������ �� ������ ����� ������ � ��� ������ - ���� �� ��� �������� ���������

        Cell cell = CurrentGameState.GetCell(cellView.Pos);
        Figure figure = CurrentGameState.GetCell(figureController.UpedFigure.Pos).figure;



        if (ItServer && CurrentGameState.EndGame == false)
        {
            //server.SendStep(new Step(figure, cell));
            var signalRClient = GameObject.Find("SignalRClient").GetComponent<SignalRClient>();
            //signalRClient.SendChat("Hello World");
            signalRClient.SendChat(TestServerHelper.ConvertToJSON(new Step(figure, cell)));
        }

        ApplyStepView(new Step(figure, cell));
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

    public void ApplyStepView(Step step)
    {
        // ��������� ��� - ���������� �� � Unity
        // �������� ������ ���� - ��������� �������� ������ �� ����� ���� � ������������� �������
        // ����� ������ �� � Model 
        // � � ����� ��������� ����� ���


        Figure figure = step.Figure;
        Cell cell = step.Cell;

        List<Cell> way = WayCalcSystem.CalcWay(CurrentGameState, figure.pos, cell.pos, figure);

        Map map = GameStateCalcSystem.ApplyStep(CurrentGameState, figure, cell);
        gameStates.Add(map);

        List<Vector3> wayVectors = new List<Vector3>();

        for (int i = 0; i < way.Count; i++)
        {
            wayVectors.Add(new Vector3(way[i].pos.X, 0f, way[i].pos.Y));
        }

        // � ������ ����� ������ -> � ����� ������
        if (cell.figure != null)
        {
            figureController.EatFigureView(cell.figure, CurrentGameState);
        }

        //figureController.AnimateMoveFigure(figureController.UpedFigure, wayVectors);
        figureController.AnimateMoveFigure(figureController.FindFigureView(figure, CurrentGameState), wayVectors);
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
        ChangeSpeedCameraConroller();

        if (CurrentGameState.EndGame == true)
        {
            EndGame();
            return;
        }

        SetFigViewForNewStep();
        SetCellViewForNewStep();

        PlayerType playerType = CurrentGameState.GetPlayerType(CurrentGameState.NumberPlayerStep);

        switch (playerType)
        {
            case PlayerType.Human:
                // ����� ������
                CornerType cornerPlayer = CurrentGameState.GetPlayerCorner(CurrentGameState.NumberPlayerStep);
                
                if (cornerPlayer == CornerType.DownLeft || cornerPlayer == CornerType.DownRight)
                    cameraController.SwitchCamera(CameraViewType.inGame1);
                else
                    cameraController.SwitchCamera(CameraViewType.inGame2);

                break;

            case PlayerType.AI:
                AIStep();
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
            DestroyAll();
        
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
        if (ItServer == true) server.CloseConnection();

        // ����� ����
        Debug.Log("����� ����");
        cameraController.SetCameraSpeed(cameraSpeed);
        cameraController.SwitchCamera(CameraViewType.noteMenu);

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

    private async void AIStep()
    {
        await Task.Run(() =>
        {
            TestAI.AlphaBeta(CurrentGameState, 0, int.MinValue, int.MaxValue);
        });

        if (gameStates.Count == 0) return;

        figureController.UpedFigure = figureController.FindFigureView(TestAI.bestFigure, CurrentGameState);
        ApplyStepView( new Step(TestAI.bestFigure, TestAI.bestCell));
    }   


    private void SetFigViewForNewStep()
    {
        // ����������� FigureContoller �� ����� ���
        // �������� � ������ ������� ������ ����� (���� ��� �������) BoxColiders � ��� �����

        figureController.UpedFigure = null;
        figureController.OFFAllBoxColiders();

        if (CurrentGameState.GetPlayerType(CurrentGameState.NumberPlayerStep)  == PlayerType.Human)
            figureController.OnBoxColiders(CurrentGameState.NumberPlayerStep);
        
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


    public void testLoad()
    {
        Map loadMap = TestSerialization.Load();

        LoadMap(loadMap);
    }

    private void ChangeSpeedCameraConroller()
    {
        if (CurrentGameState.CountStep == 2)
        {
            cameraSpeed = cameraController.GetCameraSpeed();
            cameraController.SetCameraSpeed(0f);
        }
    }

    private Step testStep;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("Save Step");
            StreamWriter file1 = File.CreateText("D://Github//GameColorChess//ColorChess//Demo_2//Assets//Code//SaveStep.json");
            file1.WriteLine(TestServerHelper.ConvertToJSON(testStep));
            file1.Close();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("Load Step");
            string data = File.ReadAllText("D://Github//GameColorChess//ColorChess//Demo_2//Assets//Code//SaveStep.json");
            Debug.Log("step:" + data);
            ApplyStepView(TestServerHelper.ConvertJSONtoSTEP(data));
        }

    }

    private bool ItServer { get 
        {
            bool itServer = false;

            foreach (var player in CurrentGameState.Players)
            {
                if (player.type == PlayerType.Online) itServer = true;
            }

            return itServer;
        } 
    }

}




/*
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
 */