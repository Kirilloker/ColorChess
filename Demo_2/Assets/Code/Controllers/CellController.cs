using ColorChessModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellController : MonoBehaviour
{
    [SerializeField]
    private GameController gameController;

    private Prefabs prefabs;
    public CellView[,] cells;

    private void Start()
    {
        prefabs = GameObject.FindWithTag("Prefabs").GetComponent<Prefabs>();
    }

    public void CreateCells(Map gameState)
    {
        GameObject prefabsCell = prefabs.GetCell();
        Transform parent = GameObject.FindWithTag("Cell").transform;

        cells = new CellView[gameState.Length, gameState.Width];

        for (int i = 0; i < gameState.Length; i++)
        {
            for (int j = 0; j < gameState.Width; j++)
            {
                GameObject cell = Instantiate(prefabsCell, new Vector3(i, 0f, j), Quaternion.AngleAxis(-90, Vector3.right), parent) as GameObject;
                cells[i, j] = cell.GetComponent<CellView>();
                cells[i, j].FindComponents();
                cells[i, j].SetCellController(this);

                if (gameState.GetCell(i, j).type != CellType.Empty)
                {
                    cells[i, j].GetComponent<MeshRenderer>().material = prefabs.GetColorCell(
                        gameState.GetColorTypeCell(i,j), gameState.GetCellType(i, j));
                } 
            }
        }

    }

    public void ChangeMaterialCell(int i, int j, Map gameState)
    {
        cells[i, j].GetComponent<MeshRenderer>().material = prefabs.GetColorCell(
        gameState.GetColorTypeCell(i, j), gameState.GetCellType(i, j));
    }

    public void OnClicked(CellView cellView)
    {
        gameController.CellOnClicked(cellView);
    }

    public void DestroyAll()
    {
        Transform parent = GameObject.FindWithTag("Cell").transform;

        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }

    public void ShowAllSteps(List<ColorChessModel.Cell> way)
    {
        HideAllPrompts();

        for (int i = 0; i < way.Count; i++)
        {
            cells[way[i].pos.X, way[i].pos.Y].ShowPrompt();
        }
    }

    public void HideAllPrompts()
    {
        for (int i = 0; i < cells.GetLength(0); i++)
        {
            for (int j = 0; j < cells.GetLength(1); j++)
            {
                cells[i, j].HidePrompt();
            }
        }
    }

    public void OFFALLBoxColiders()
    {
        foreach (CellView cell in cells)
        {
            cell.OFFBoxColider();
        }
    }

    public void OnBoxColidersForList(List<Cell> way)
    {
        OFFALLBoxColiders();

        for (int i = 0; i < way.Count; i++)
        {
            cells[way[i].pos.X, way[i].pos.Y].ONBoxColider();
        }
    }
    public bool GetBoolFigureInCell(Position position)
    {
        return gameController.GetBoolFigureInCell(position);
    }
}
