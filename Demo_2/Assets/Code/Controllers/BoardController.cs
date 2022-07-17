using ColorChessModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    [SerializeField]
    private GameController gameController;
    [SerializeField]
    private GameObject boardDecor;

    private GameObject board;

    private Prefabs prefabs;

    private void Awake()
    {
        prefabs = GameObject.FindWithTag("Prefabs").GetComponent<Prefabs>();
    }

    public void CreateBoard(Map gameState)
    {
        if(board != null)
        {
            GameObject.Destroy(board);
        }

        boardDecor.SetActive(false);

        Transform parent = GameObject.FindWithTag("Arena").transform;
        GameObject prefabsBoard = prefabs.GetBoard();
        board = Instantiate(prefabsBoard, parent.transform.localPosition, Quaternion.AngleAxis(270, Vector3.up), parent);

        // ���������
        GameObject.FindWithTag("Game").transform.localScale = new Vector3(Mathf.Pow(0.9f, (float)(gameState.Width - 9)), Mathf.Pow(0.9f, (float)(gameState.Width - 9)), Mathf.Pow(0.9f, (float)(gameState.Length - 9)));

        board.transform.localScale = new Vector3(gameState.Length, gameState.Length, gameState.Width);
        board.transform.localPosition = new Vector3((gameState.Width - 1f) / 2f, -0.1f, (gameState.Width - 1f) / 2f); 
        board.tag = "Board";
        board.name = "Board";

        ChangeColor(gameState);
    }


    private void ChangeColor(Map gameState)
    {
        // ���������
        List<ColorChessModel.Player> players = gameState.PLayers;

        var material_board = board.GetComponentInChildren<MeshRenderer>().materials;

        foreach (ColorChessModel.Player player in players)
        {
            material_board[(int) player.corner] = prefabs.GetColor(player.color);
        }

        if (players.Count == 2 && players[0].corner == CornerType.DownLeft && players[1].corner == CornerType.UpRight)
        {
            material_board[2] = prefabs.GetColor(players[0].color);
            material_board[4] = prefabs.GetColor(players[1].color);
        }

        board.GetComponentInChildren<MeshRenderer>().materials = material_board;
    }

    public void Destroy()
    {
        GameObject.Destroy(board);
    }

}