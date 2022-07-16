using ColorChessModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FigureController : MonoBehaviour
{
    [SerializeField]
    private GameController gameController;
    private Prefabs prefabs;
    private List<List<FigureView>> figures = new List<List<FigureView>>();

    private FigureView upedFigure;

    private void Awake()
    {
        prefabs = GameObject.FindWithTag("Prefabs").GetComponent<Prefabs>();
    }


    public void AnimateMoveFigure(FigureView figure, List<Vector3> vector3)
    {
        StartCoroutine(figure.AnimateMove(vector3));
    }

    public void DestroyAll()
    {
        foreach (List<FigureView> player in figures)
        {
            foreach (FigureView figure in player)
            {
                //
            }
        }

        upedFigure = null;
    }


    public void CreateFigures(Map gameState)
    {
        Transform parent = GameObject.FindWithTag("Figure").transform;

        // »—œ–¿¬»“‹
        foreach (ColorChessModel.Player player in gameState.players)
        {
            List<FigureView> figuresViewList = new List<FigureView>();

            foreach (ColorChessModel.Figure figure in player.figures)
            {
                GameObject prefabsFigure = prefabs.GetFigure(figure.type);

                GameObject figureGameObject = Instantiate(prefabsFigure, parent.transform.localPosition, Quaternion.AngleAxis(270, Vector3.up), parent);

                Material[] materialFigure = figureGameObject.GetComponent<MeshRenderer>().materials;
                materialFigure[1] = prefabs.GetColor(player.color);
                figureGameObject.GetComponent<MeshRenderer>().materials = materialFigure;

                FigureView figureView = figureGameObject.GetComponent<FigureView>();
                figureView.SetNumberPlayer(player.number);
                figureView.Pos = figure.pos;
                figureView.SetType(figure.type);
                figureView.SetFigureController(this);

                if (figure.type == FigureType.Horse)
                {
                    if (player.corner == CornerType.UpLeft || player.corner == CornerType.UpRight)
                    {
                        figureView.SetRotation(180);
                    }
                    else
                    {
                        figureView.SetRotation(0);
                    }
                }

                figureGameObject.name = TypeToString.ToString(figure.type) + player.number;

                figuresViewList.Add(figureView);
            }

            figures.Add(figuresViewList);
        }
    }

    public void OnClicked(FigureView figureView)
    {
        if (upedFigure != null)
        {
            upedFigure.Down();
        }

        figureView.Up();
        upedFigure = figureView;

        gameController.FigureOnClicked(figureView);
    }

    public void OnBoxColiders(int numberPlayer)
    {
        foreach (FigureView figure in figures[numberPlayer])
        {
            figure.StateBoxColodier(true);
        }
    }

    public void OffBoxColidersPlayers(int numberPlayer)
    {
        foreach (FigureView figure in figures[numberPlayer])
        {
            figure.StateBoxColodier(false);
        }
    }

    public void OFFAllBoxColiders()
    {
        for (int i = 0; i < figures.Count; i++)
        {
            OffBoxColidersPlayers(i);
        }
    }

    public FigureView UpedFigure { get { return upedFigure; } set { upedFigure = value; } }
}
