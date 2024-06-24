using ColorChessModel;
using System.Collections.Generic;
using UnityEngine;

public class FigureController : MonoBehaviour
{
    private MainController mainController;
    [SerializeField]
    private AudioController audioController;

    private Prefabs prefabs;
    private List<List<FigureView>> figures = new List<List<FigureView>>();

    private FigureView upedFigure;

    private Transform transformFigure;

    public bool LowGraphics;

    private void Awake()
    {
        prefabs = GameObject.FindWithTag("Prefabs").GetComponent<Prefabs>();
        transformFigure = GameObject.FindWithTag("Figure").transform;
        mainController = MainController.Instance;
    }

    public void AnimateMoveFigure(FigureView figure, List<Vector3> vectorWay)
    {
        // �������� ����� ����� ��������� ���������� ���� � ��������� ����
        // ���� ��������� ������ ������� �� ������ ������ � ��������� ���, ����� ���������������
        if (LowGraphics == false)
            StartCoroutine(figure.AnimateMove(vectorWay));
        else
            figure.Move(vectorWay[vectorWay.Count - 1]);

        audioController.PlayAudio(SoundType.Step);
    }

    public void DestroyAll()
    {
        foreach (Transform child in transformFigure)
            Destroy(child.gameObject);

        upedFigure = null;
    }

    public void CreateFigures(Map gameState)
    {
        figures = new List<List<FigureView>>();

        foreach (Player player in gameState.Players)
        {
            List<FigureView> figuresViewList = new List<FigureView>();

            foreach (Figure figure in player.Figures)
                figuresViewList.Add(CreateFigure(figure, player.Color, player.Corner));

            figures.Add(figuresViewList);
        }
    }

    public FigureView CreateFigure(Figure figure, ColorType color, CornerType corner)
    {
        GameObject prefabsFigure = prefabs.GetFigure(figure.Type);

        GameObject figureGameObject = Instantiate(prefabsFigure, transformFigure.transform.localPosition, Quaternion.AngleAxis(270, Vector3.up), transformFigure);

        Material[] materialFigure = figureGameObject.GetComponent<MeshRenderer>().materials;
        materialFigure[1] = prefabs.GetColor(color);
        figureGameObject.GetComponent<MeshRenderer>().materials = materialFigure;

        FigureView figureView = figureGameObject.GetComponent<FigureView>();
        figureView.FindComponents();
        figureView.SetNumberPlayer(figure.Number);
        figureView.Pos = figure.Pos;
        figureView.SetType(figure.Type);
        figureView.SetFigureController(this);

        if (figure.Type == FigureType.Horse)
        {
            if (corner == CornerType.UpLeft || corner == CornerType.UpRight)
                figureView.SetRotation(180);
            else
                figureView.SetRotation(0);
        }

        figureGameObject.name = TypeToString.ToString(figure.Type) + figure.Number;

        return figureView;
    }

    public void OnClicked(FigureView figureView)
    {
        // ������� �� ������
        // �������� �������� ������ �� ����� (���� ����� ����)
        // ��������� ��������� 

        if (upedFigure != null)
            upedFigure.Down();
        

        figureView.Up();
        upedFigure = figureView;

        mainController.FigureSelected(figureView.Pos);
    }

    public FigureView FindFigureView(Figure figureModel, Map gameState)
    {
        foreach (List<FigureView> player in figures)
            foreach (FigureView figure in player)
                if (figure.Pos == figureModel.Pos)
                    return figure;

        return new FigureView();
    }

    public void EatFigureView(Figure figure, Map gameState)
    {
        // �������� ������
        // ������� �� ������ ����� � ������� ������ �� �����

        FigureView figureView = FindFigureView(figure, gameState);
        figures[figure.Number].Remove(figureView);
        Destroy(figureView.gameObject);
    }

    public void OnBoxColliders(int numberPlayer)
    {
        foreach (FigureView figure in figures[numberPlayer])
            figure.StateBoxColodier(true);
    }

    public void OffBoxCollidersPlayers(int numberPlayer)
    {
        foreach (FigureView figure in figures[numberPlayer])
            figure.StateBoxColodier(false);
    }

    public void OFFAllBoxColliders()
    {
        for (int i = 0; i < figures.Count; i++)
            OffBoxCollidersPlayers(i);
    }

    public FigureView UpedFigure { get { return upedFigure; } set { upedFigure = value; } }

}
