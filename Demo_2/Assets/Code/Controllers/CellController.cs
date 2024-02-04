using ColorChessModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellController : MonoBehaviour
{
    private MainController mainController;

    private Prefabs prefabs;
    public CellView[,] cells;

    private void Start()
    {
        prefabs = GameObject.FindWithTag("Prefabs").GetComponent<Prefabs>();
        mainController = MainController.Instance;
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
                GameObject cell = Instantiate(prefabsCell, new Vector3(i, 0f, j), Quaternion.AngleAxis(-90, Vector3.right)) as GameObject;
                cell.transform.SetParent(parent.transform);
                cell.transform.localPosition = new Vector3(i, 0f, j);
                cell.transform.localScale = new Vector3(50f, 50f, 50f);
                cells[i, j] = cell.GetComponent<CellView>();
                cells[i, j].FindComponents();
                cells[i, j].SetCellController(this);

                if (gameState.GetCell(i, j).Type != CellType.Empty)
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
        mainController.CellSelected(cellView.Pos);
    }

    public void DestroyAll()
    {
        Transform parent = GameObject.FindWithTag("Cell").transform;

        foreach (Transform child in parent)
            Destroy(child.gameObject);
    }

    public void ShowAllSteps(List<Cell> way)
    {
        HideAllPrompts();

        for (int i = 0; i < way.Count; i++)
            cells[way[i].Pos.X, way[i].Pos.Y].ShowPrompt();
    }

    public void HideAllPrompts()
    {
        for (int i = 0; i < cells.GetLength(0); i++)
            for (int j = 0; j < cells.GetLength(1); j++)
                cells[i, j].HidePrompt();
    }

    public void OFFALLBoxColliders()
    {
        foreach (CellView cell in cells)
            cell.OFFBoxColider();
    }

    public void OnBoxCollidersForList(List<Cell> way)
    {
        OFFALLBoxColliders();

        for (int i = 0; i < way.Count; i++)
            cells[way[i].Pos.X, way[i].Pos.Y].ONBoxColider();
    }
    public bool GetBoolFigureInCell(Position position)
    {
        return mainController.GetBoolFigureInCell(position);
    }
}
