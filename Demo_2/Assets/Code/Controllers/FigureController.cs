using ColorChessModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FigureController : MonoBehaviour
{
    private Prefabs prefabs;

    private void Awake()
    {
        prefabs = GameObject.FindWithTag("Prefabs").GetComponent<Prefabs>();
    }


    public void CreateFigures(Map gameState)
    {

    }
}
