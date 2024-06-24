using ColorChessModel;
using UnityEngine;
using UnityEngine.UI;

public class BoardUIController : MonoBehaviour
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

        for (int i = 0; i < gameState.PlayersCount; i++)
        {
            SetScoreInCorner(gameState.GetPlayerCorner(i), gameState.GetPlayerFiguresCount(i));
            countAllFigure += gameState.GetPlayerFiguresCount(i);
        }

        SetEmptyCell(gameState.Length * gameState.Width - countAllFigure);

        // ���� �� ����� ����������� ������������ (2 ������)
        if (gameState.PlayersCount == 2 && gameState.GetPlayerCorner(0) == CornerType.DownLeft && gameState.GetPlayerCorner(1) == CornerType.UpRight)
        {
            SetScoreInCorner(CornerType.DownRight, gameState.GetPlayerFiguresCount(1));
            SetScoreInCorner(CornerType.UpLeft, gameState.GetPlayerFiguresCount(0));
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

        for (int i = 0; i < gameState.PlayersCount; i++)
            SetScoreInCorner(gameState.GetPlayerCorner(i), gameState.GetScorePlayer(i));


        // ���� �� ����� ����������� ������������ (2 ������)
        if (gameState.PlayersCount == 2 && gameState.GetPlayerCorner(0) == CornerType.DownLeft && gameState.GetPlayerCorner(1) == CornerType.UpRight)
        {
            SetScoreInCorner(CornerType.DownRight, gameState.GetScorePlayer(1));
            SetScoreInCorner(CornerType.UpLeft, gameState.GetScorePlayer(0));
        }
    }
}
