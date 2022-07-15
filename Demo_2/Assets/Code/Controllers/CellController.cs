using ColorChessModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellController : MonoBehaviour
{
    private Prefabs prefabs;
    public CellView[,] cells;


    private void Awake()
    {
        prefabs = GameObject.FindWithTag("Prefabs").GetComponent<Prefabs>();
    }

    public void CreateCells(Map gameState)
    {
        GameObject prefabsCell = prefabs.GetCell();
        Transform parent = GameObject.FindWithTag("Cell").transform;

        cells = new CellView[gameState.Length, gameState.Width];

        // ��������� - �� ������� ������ �� ��� ����� ������ ����� CELL
        for (int i = 0; i < gameState.Length; i++)
        {
            for (int j = 0; j < gameState.Width; j++)
            {
                GameObject cell = Instantiate(prefabsCell, new Vector3(i, 0f, j), Quaternion.AngleAxis(-90, Vector3.right), parent) as GameObject;
                cells[i, j] = cell.GetComponent<CellView>();
                cells[i, j].SetPos(new Position(i, j));

                if (gameState.cells[i, j].type != CellType.Empty)
                {
                    cells[i, j].GetComponent<MeshRenderer>().material = prefabs.GetColorCell(
                        gameState.GetColorTypeCell(i,j), gameState.GetCellType(i, j));
                } 
            }
        }

    }
}
