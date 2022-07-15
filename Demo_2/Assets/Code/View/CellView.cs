using ColorChessModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellView : MonoBehaviour
{
    Position pos;
    MeshRenderer cellMesh;
    GameObject prompt;

    private void Awake()
    {
        cellMesh = this.GetComponent<MeshRenderer>();
        prompt = this.transform.GetChild(0).gameObject;
    }

    public void SetPos(Position _pos)
    {
        pos = _pos;
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



}
