using System.Collections.Generic;
using UnityEngine;
using ColorChessModel;
using System.Threading.Tasks;

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
    [SerializeField]
    private ServerUI serverUI;

    private bool IsFirstGame = true;

    private float cameraSpeed;

    // ������ ������� � AI
    private IAI[] ai;

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
        SwitchCameraStartGame();
        uiController.ViewUIGame(true);
        serverUI.StartGame();

        InitAI();

        StartNewStep();
    }

    void SwitchCameraStartGame() 
    {
        foreach (var player in CurrentGameState.Players)
        {
            if (player.Type == PlayerType.Human)
            {
                if (player.Corner == CornerType.DownLeft || player.Corner == CornerType.DownRight) 
                    cameraController.SwitchCamera(CameraViewType.inGame1);
                else
                    cameraController.SwitchCamera(CameraViewType.inGame2);
                return;
            }
        }
    }

    void InitAI() 
    {
        ai = new IAI[CurrentGameState.PlayersCount];

        for (int i = 0; i < CurrentGameState.PlayersCount; i++)
        {
            if (CurrentGameState.GetPlayerType(i) == PlayerType.AI) 
                ai[i] = new MinMaxAI();
            else if (CurrentGameState.GetPlayerType(i) == PlayerType.AI2)
                ai[i] = new MonteCarloAI();
        }
    }


    public void CellOnClicked(CellView cellView)
    {
        // ������ �� ������ ����� ������ � ��� ������ - ���� �� ��� �������� ���������
        Cell cell = CurrentGameState.GetCell(cellView.Pos);
        Figure figure = CurrentGameState.GetCell(figureController.UpedFigure.Pos).Figure;
        Step step = new Step(figure, cell);

        ApplyStepView(step);
    }

    public void FigureOnClicked(FigureView figureView)
    {
        // �������� ������ �� ������� ������, ������� ��� �� ��� ��������� ����
        // � �������� ��������� (� ��� �� BoxColiders � ������, �� ������� ���������� ���������)

        Figure selectFigure = CurrentGameState.GetCell(figureView.Pos).Figure;

        if (selectFigure != null)
        {
            List<Cell> allSteps = WayCalcSystem.CalcAllSteps(CurrentGameState, selectFigure);
            cellController.ShowAllSteps(allSteps);
            cellController.OnBoxColidersForList(allSteps);
        }
        else
        {
            Print.Log("����� ������ �� �������");
        }
    }

    public void ApplyStepView(Step step)
    {
        // ��������� ��� - ���������� �� � Unity
        // �������� ������ ���� - ��������� �������� ������ �� ����� ���� � ������������� ������
        // ����� ������ �� � Model 
        // � � ����� ��������� ����� ���

        Figure figure = step.Figure;
        Cell cell = step.Cell;

        List<Cell> way = WayCalcSystem.CalcWay(CurrentGameState, figure.Pos, cell.Pos, figure);
        
        if (IsServer && CurrentGameState.Players[CurrentGameState.NumberPlayerStep].Type == PlayerType.Human) 
        {
            if (CurrentGameState.EndGame == true)
                server.SendLastStep(step);
            else
                server.SendStep(step);

        } 
            

        Map map = GameStateCalcSystem.ApplyStep(CurrentGameState, figure, cell);
        gameStates.Add(map);


        List<Vector3> wayVectors = new List<Vector3>();

        for (int i = 0; i < way.Count; i++)
            wayVectors.Add(new Vector3(way[i].Pos.X, 0f, way[i].Pos.Y));

        // � ������ ����� ������ -> � ����� ������
        if (cell.Figure != null)
            figureController.EatFigureView(cell.Figure, CurrentGameState);

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
                    if (PreviousvGameState.GetCell(i, j).Type != CellType.Dark &&
                        CurrentGameState.GetCell(i, j).Type == CellType.Dark)
                    {
                        SoundCell = 1;
                    }

                    // ���� ������ ������������� �� Dark 
                    if (PreviousvGameState.GetCell(i, j).Type == CellType.Dark &&
                        CurrentGameState.GetCell(i, j).Type != CellType.Dark)
                    {
                        SoundCell = 2;
                    }
                }
            }
        }

        if (SoundCell == 1)
            audioController.PlayAudio(SoundType.DarkCapture);
        if (SoundCell == 2)
            audioController.PlayAudio(SoundType.ReverseDarkCapture);

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

    public void SelectCustomGameMode(int sizeMap, PlayerType[] typePlayer, CornerType[] cornerPlayer, ColorType[] colorPlayer) 
    {
        gameStateBuilder.SetCustomGameState(sizeMap, typePlayer, cornerPlayer, colorPlayer);
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
                AIStep(playerType);
                break;             
            case PlayerType.AI2:
                AIStep(playerType);
                break;             
            case PlayerType.Online:
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

    public void Surrender() 
    {
        if (IsServer == true)
        {
            
        }

        EndGame();
    
    }

    public void EndGame()
    {
        if (IsServer == true)
        {
            //uiController.OnlineGameExit();
            server.CloseConnection();
        }

        // ����� ����
        Print.Log("����� ����");
        uiController.ViewUIGame(false);
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
    private async void AIStep(PlayerType AIType)
    {
        Step step = new();

        await Task.Run(() =>
        {
            step = ai[CurrentGameState.NumberPlayerStep].getStep(CurrentGameState);
        });

        if (gameStates.Count == 0) return;

        figureController.UpedFigure = figureController.FindFigureView(step.Figure, CurrentGameState);
        ApplyStepView(step);
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
        return CurrentGameState.GetCell(position).Figure != null;
    }
    public Map CurrentGameState { get { return gameStates[gameStates.Count - 1]; } }
    public Map PreviousvGameState { get { return gameStates[gameStates.Count - 2]; } }


    private void ChangeSpeedCameraConroller()
    {
        if (CurrentGameState.CountStep == 2)
        {
            cameraSpeed = cameraController.GetCameraSpeed();
            cameraController.SetCameraSpeed(0f);
        }
    }

    private bool IsServer { get 
        {
            foreach (var player in CurrentGameState.Players)
                if (player.Type == PlayerType.Online) return true;
            
            return false;
        } 
    }

}
