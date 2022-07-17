using ColorChessModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellView : MonoBehaviour
{
    private CellController cellController;
    private GameObject prompt;

    private MeshRenderer cellMesh;
    private BoxCollider boxCollider;

    public void FindComponents()
    {
        cellMesh = this.GetComponent<MeshRenderer>();
        boxCollider = this.GetComponent<BoxCollider>();
        prompt = this.transform.GetChild(0).gameObject;
    }

    private void OnMouseUpAsButton()
    {
        cellController.OnClicked(this);
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


    public Position Pos
    {
        get { return new Position(transform.localPosition.x, transform.localPosition.z); }
        set { this.transform.localPosition = new Vector3(value.X, 0, value.Y); }
    }

}
