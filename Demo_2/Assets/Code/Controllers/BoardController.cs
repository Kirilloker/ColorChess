using ColorChessModel;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    [SerializeField]
    private GameObject boardDecor;

    private GameObject board;

    private Prefabs prefabs;

    private BoardUIController boardUI;

    private void Awake()
    {
        prefabs = GameObject.FindWithTag("Prefabs").GetComponent<Prefabs>();
    }

    public void CreateBoard(Map gameState)
    {
        if (board != null)
            Destroy(board);
        
        HideBoardDecor();

        Transform parent = GameObject.FindWithTag("Arena").transform;
        GameObject prefabsBoard = prefabs.GetBoard();
        board = Instantiate(prefabsBoard, parent.transform.localPosition, Quaternion.AngleAxis(270, Vector3.up), parent);

        // ���������
        GameObject.FindWithTag("Game").transform.localScale = new Vector3(Mathf.Pow(0.9f, (float)(gameState.Width - 9)), Mathf.Pow(0.9f, (float)(gameState.Width - 9)), Mathf.Pow(0.9f, (float)(gameState.Length - 9)));

        board.transform.localScale = new Vector3(gameState.Length, gameState.Length, gameState.Width);
        board.transform.localPosition = new Vector3((gameState.Width - 1f) / 2f, -0.1f, (gameState.Width - 1f) / 2f); 
        board.tag = "Board";
        board.name = "Board";

        boardUI = board.GetComponentInChildren<BoardUIController>();
        boardUI.FirstSet(gameState);

        ChangeColor(gameState);
    }

    public void SetScoreUI(Map map)
    {
        boardUI.SetScore(map);
    }

    private void ChangeColor(Map gameState)
    {
        // �������� ���� ����� �����

        List<Player> players = gameState.Players;

        var material_board = board.GetComponentInChildren<MeshRenderer>().materials;

        foreach (Player player in players)
            material_board[(int) player.Corner + 1] = prefabs.GetColor(player.Color);

        if (players.Count == 2 && players[0].Corner == CornerType.DownLeft && players[1].Corner == CornerType.UpRight)
        {
            material_board[2] = prefabs.GetColor(players[1].Color);
            material_board[4] = prefabs.GetColor(players[0].Color);
        }

        board.GetComponentInChildren<MeshRenderer>().materials = material_board;
    }

    public void Destroy()
    {
        Destroy(board);
    }


    public void HideBoardDecor()
    {
        boardDecor.SetActive(false);
    }

    public void ShowBoardDecor()
    {
        boardDecor.SetActive(true);
    }
}
