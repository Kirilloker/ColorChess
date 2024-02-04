using System.Collections.Generic;
using UnityEngine;
using ColorChessModel;

public class GameController : MonoBehaviour, IGameController
{
    private bool IsFirstGame = true;
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
    private ServerUI serverUI;

    private float cameraSpeed;

    private void Start()
    {
        MainController.Instance.SetGameController(this);
    }

    public void PreparingForStartGame(Map map)
    {
        CheckNewGame();
        boardController.CreateBoard(map);
        cellController.CreateCells(map);
        figureController.CreateFigures(map);
        SwitchCameraStartGame(map);
        uiController.ViewUIGame(true);
        serverUI.StartGame();
    }

    public void PreparingForNewStep(Map map)
    {
        ChangeSpeedCameraController(map);
        SetFigViewForNewStep(map);
        SetCellViewForNewStep();
    }

    public void EndGame()
    {
        uiController.ViewUIGame(false);
        cameraController.SetCameraSpeed(cameraSpeed);
        cameraController.SwitchCamera(CameraViewType.noteMenu);
        figureController.UpedFigure = null;
        figureController.OFFAllBoxColliders();
        cellController.OFFALLBoxColliders();
        IsFirstGame = false;
    }

    public void DrawNewGameState(Map previousMap, Map currentMap)
    {
        // Если состояние клетки в модели изменилось по сравнению с предыдущим состоянием
        // То меняем у неё цвет
        // А так же меняем Очки на UI-board

        // 0 - ничего не изменилось 1 - появилась захваченная клетка 2 - исчезла захваченная клетка
        int SoundCell = 0;

        for (int i = 0; i < currentMap.Length; i++)
        {
            for (int j = 0; j < currentMap.Width; j++)
            {
                if (currentMap.GetCell(i, j) != previousMap.GetCell(i, j))
                {
                    cellController.ChangeMaterialCell(i, j, currentMap);

                    // Если клетка перекрасилась в Dark
                    if (previousMap.GetCell(i, j).Type != CellType.Dark &&
                        currentMap.GetCell(i, j).Type == CellType.Dark)
                    {
                        SoundCell = 1;
                    }

                    // Если клетка перекрасилась из Dark 
                    if (previousMap.GetCell(i, j).Type == CellType.Dark &&
                        currentMap.GetCell(i, j).Type != CellType.Dark)
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

        boardController.SetScoreUI(currentMap);
    }

    public void EatFigure(Figure figure, Map map)
    {
        figureController.EatFigureView(figure, map);
    }

    public void PreparingForEndStep(Figure figure, Map map, List<Cell> way)
    {
        List<Vector3> wayVectors = new List<Vector3>();

        for (int i = 0; i < way.Count; i++)
            wayVectors.Add(new Vector3(way[i].Pos.X, 0f, way[i].Pos.Y));

        figureController.AnimateMoveFigure(figureController.FindFigureView(figure, map), wayVectors);
        cellController.HideAllPrompts();
    }

    public void HumanPlayerStartStep(Map map)
    {
        CornerType cornerPlayer = map.GetPlayerCorner(map.NumberPlayerStep);

        if (cornerPlayer == CornerType.DownLeft || cornerPlayer == CornerType.DownRight)
            cameraController.SwitchCamera(CameraViewType.inGame1);
        else
            cameraController.SwitchCamera(CameraViewType.inGame2);
    }

    private void SetFigViewForNewStep(Map map)
    {
        figureController.UpedFigure = null;
        figureController.OFFAllBoxColliders();

        if (map.GetPlayerType(map.NumberPlayerStep) == PlayerType.Human)
            figureController.OnBoxColliders(map.NumberPlayerStep);
    }

    private void SetCellViewForNewStep()
    {
        cellController.OFFALLBoxColliders();
        cellController.HideAllPrompts();
    }

    private void ChangeSpeedCameraController(Map map)
    {
        if (map.CountStep == 2)
        {
            cameraSpeed = cameraController.GetCameraSpeed();
            cameraController.SetCameraSpeed(0f);
        }
    }

    private void CheckNewGame()
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

    void SwitchCameraStartGame(Map map)
    {
        foreach (var player in map.Players)
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

    public void FigureSelected(Map map, Figure figure)
    {
        List<Cell> allSteps = WayCalcSystem.CalcAllSteps(map, figure);
        cellController.ShowAllSteps(allSteps);
        cellController.OnBoxCollidersForList(allSteps);
    }


    public Position GetPositionSelectedFigure()
    {
        return figureController.UpedFigure.Pos;
    }

    public void AICalcComplete(Figure figure, Map map)
    {
        figureController.UpedFigure = figureController.FindFigureView(figure, map);
    }
}