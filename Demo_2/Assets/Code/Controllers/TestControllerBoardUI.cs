using ColorChessModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestControllerBoardUI : MonoBehaviour
{
    // ���������� ������ ������
    [SerializeField]
    private Text emptyCellText1;
    [SerializeField]
    private Text emptyCellText2;

    // ����� �� ����� �����
    [SerializeField]
    private Text[] cornerText = new Text[4];

    public void FirstSet(Map gameState)
    {
        // ��������� (�� �� ������� �� � ������) ������� ���-�� ���� ��������� ������� �����
        // ��-�� ���� ��� ��� �������� ����� �� �������� ���������� ����, ��� ������������� ����� ������� ��� ����������
        
        int countAllFigure = 0;

        foreach (var player in gameState.players)
        {
            SetScoreInCorner(player.corner, player.figures.Count);
            countAllFigure += player.figures.Count;
        }
        SetEmptyCell(gameState.Length * gameState.Width - countAllFigure);

        // ���� �� ����� ������������ ������������ (2 ������)
        if (gameState.players.Count == 2 && gameState.players[0].corner == CornerType.DownLeft && gameState.players[1].corner == CornerType.UpRight)
        {
            SetScoreInCorner(CornerType.DownRight, gameState.PLayers[1].figures.Count);
            SetScoreInCorner(CornerType.UpLeft, gameState.PLayers[0].figures.Count);
        }
    }

    private void SetScoreInCorner(CornerType corner, int score)
    {
        cornerText[(int)corner].text = score.ToString();
    }

    private void SetEmptyCell(int countEmptyCell)
    {
        emptyCellText1.text = countEmptyCell.ToString();
        emptyCellText2.text = countEmptyCell.ToString();
    }

    public void SetScore(Map gameState)
    {
        // ������������� ���� ������� � ���������� ������ ������

        SetEmptyCell(gameState.CountEmptyCell);

        foreach (var player in gameState.players)
        {
            SetScoreInCorner(player.corner, gameState.GetScorePlayer(player.number));
        }

        // ���� �� ����� ������������ ������������ (2 ������)
        if (gameState.players.Count == 2 && gameState.players[0].corner == CornerType.DownLeft && gameState.players[1].corner == CornerType.UpRight)
        {
            SetScoreInCorner(CornerType.DownRight, gameState.GetScorePlayer(gameState.players[1].number));
            SetScoreInCorner(CornerType.UpLeft, gameState.GetScorePlayer(gameState.players[0].number));
        }
    }
}
