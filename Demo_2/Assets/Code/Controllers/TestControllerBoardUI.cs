using ColorChessModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestControllerBoardUI : MonoBehaviour
{
    // Количество пустых клеток
    [SerializeField]
    private Text emptyCellText1;
    [SerializeField]
    private Text emptyCellText2;

    // Текст на углах доски
    [SerializeField]
    private Text[] cornerText = new Text[4];

    public void FirstSet(Map gameState)
    {
        // ИСПРАВИТЬ (но не слишком то и срочно) Сломает что-то если изменится подсчёт очков
        // Из-за того что при создание доски не успевает просчитать очки, при инициализации нужно вручную его выставлять
        
        int countAllFigure = 0;

        for (int i = 0; i < gameState.PlayersCount; i++)
        {
            SetScoreInCorner(gameState.GetPlayerCorner(i), gameState.GetPlayerFiguresCount(i));
            countAllFigure += gameState.GetPlayerFiguresCount(i);
        }

        SetEmptyCell(gameState.Length * gameState.Width - countAllFigure);

        // Если на карте страндартное расположение (2 игрока)
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
        // Устанавливает очки игроков и количество пустых клеток

        SetEmptyCell(gameState.CountEmptyCell);

        for (int i = 0; i < gameState.PlayersCount; i++)
        {
            SetScoreInCorner(gameState.GetPlayerCorner(i), gameState.GetScorePlayer(i));
        }


        // Если на карте страндартное расположение (2 игрока)
        if (gameState.PlayersCount == 2 && gameState.GetPlayerCorner(0) == CornerType.DownLeft && gameState.GetPlayerCorner(1) == CornerType.UpRight)
        {
            SetScoreInCorner(CornerType.DownRight, gameState.GetScorePlayer(1));
            SetScoreInCorner(CornerType.UpLeft, gameState.GetScorePlayer(0));
        }
    }
}
