using ColorChessModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellView : MonoBehaviour
{
    private Position pos;
    private MeshRenderer cellMesh;
    private CellController cellController;
    private GameObject prompt;

    private BoxCollider boxCollider;

    private void Awake()
    {
        cellMesh = this.GetComponent<MeshRenderer>();
        boxCollider = this.GetComponent<BoxCollider>();
        prompt = this.transform.GetChild(0).gameObject;
    }

    private void OnMouseUpAsButton()
    {
        cellController.OnClicked(this);
    }


    public void SetPos(Position _pos)
    {
        pos = _pos;
    }

    public void SetCellController(CellController _cellController)
    {
        cellController = _cellController;
    }

    public void ChangeColor(Material material)
    {
        cellMesh.material = material;
    }

    public void ShowPrompt()
    {
        prompt.SetActive(true);
    }

    public void HidePrompt()
    {
        prompt.SetActive(false);
    }

    public void OFFBoxColider()
    {
        boxCollider.enabled = false;
    }
    

    public void ONBoxColider()
    {
        boxCollider.enabled = true;
    }

    public Position Pos { get { return pos; } }

}
