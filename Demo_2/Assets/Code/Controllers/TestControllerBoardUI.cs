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

        foreach (var player in gameState.players)
        {
            SetScoreInCorner(player.corner, player.figures.Count);
            countAllFigure += player.figures.Count;
        }
        SetEmptyCell(gameState.Length * gameState.Width - countAllFigure);

        // Если на карте страндартное расположение (2 игрока)
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
        // Устанавливает очки игроков и количество пустых клеток

        SetEmptyCell(gameState.CountEmptyCell);

        foreach (var player in gameState.players)
        {
            SetScoreInCorner(player.corner, gameState.GetScorePlayer(player.number));
        }

        // Если на карте страндартное расположение (2 игрока)
        if (gameState.players.Count == 2 && gameState.players[0].corner == CornerType.DownLeft && gameState.players[1].corner == CornerType.UpRight)
        {
            SetScoreInCorner(CornerType.DownRight, gameState.GetScorePlayer(gameState.players[1].number));
            SetScoreInCorner(CornerType.UpLeft, gameState.GetScorePlayer(gameState.players[0].number));
        }
    }
}
