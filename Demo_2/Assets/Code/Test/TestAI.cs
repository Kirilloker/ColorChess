using ColorChessModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
//using MathNet.Numerics.Distributions;

public class TestAI : IAI
{
    // Глубина дерева
    int MAX_LEVEL = 4;

    // Лучший ход
    public Cell bestCell = null;
    public Figure bestFigure = null;

    // Параметры ОФ
    int pricePaint = 9;
    int priceDark = 10;
    int priceKill = 70;
    int priceAroundEnemyPawn = 60;

    // pawn - castle - bishop - king - horse - queen 
    float[] figurePercent = { 0, 0, 0, 0, 0, 0 };
    float[] figurePercentStart = { 1f, 0.5f, 0.1f, 0.4f, 0.4f, 1f };
    float[] figurePercentSpeed = { 0.5f, 0.4f, 0.1f, 0.3f, 0.3f, 1f };
    float[] figurePercentMidle = { 0.4f, 0.4f, 0.3f, 0.2f, 0.2f, 0.7f };
    float[] figurePercentSlow = { 1f, 0.4f, 0.4f, 0.2f, 0.3f, 0.5f };
    float[] figurePercentEnd = { 0.3f, 0.3f, 0.5f, 0.2f, 0.2f, 0.3f };

    // Номер бота
    int myNumber = 0;
    // Количество человек в игре 
    int playerCount = 0;
    List<int> AnotherPlayer;

    // Таймер, который следит за временем выполнение Alpha-Beta
    Stopwatch timer = new();
    // Время, выделенное на расчет Alpha-Beta
    float TIMER = 8f;
    // Вспомогательная переменная, которая останавливает расчет в случае долгого выполнения
    bool stop = false;

    public TestAI() 
    {

    }

    private List<List<Cell>> GetAvaibleForPlayer(Map map, int numberPlayer)
    {
        // Возвращает двумерный массив: первый индекс - i-ая фигура игрока; 
        // второй индекс - j-ая клетка на которую может сходить фигура

        List<List<Cell>> avaiblePlayer = new();

        Player player = map.Players[numberPlayer];

        foreach (Figure figure in player.figures)
            avaiblePlayer.Add(WayCalcSystem.CalcAllSteps(map, figure));

        return avaiblePlayer;
    }

    private void SwitchGameSutuation(float percentEmptyCell)
    {
        // Меняет стадию игры Начало-Разгон-Середина-Замедление-Конец
        if (percentEmptyCell < 0.25)
            figurePercent = figurePercentStart;
        else if (percentEmptyCell < 0.4)
            figurePercent = figurePercentSpeed;
        else if (percentEmptyCell < 0.7)
            figurePercent = figurePercentMidle;
        else if (percentEmptyCell < 0.9)
            figurePercent = figurePercentSlow;
        else if (percentEmptyCell < 1)
            figurePercent = figurePercentEnd;
    }

    private int AlphaBeta(Map map, int level, int alpha, int beta)
    {
        // Список всех возможных ходов для определенного игрока
        List<List<Cell>> avaible = new();

        // Конец игры
        if (map.EndGame == true)
        {
            int total = 0;

            foreach (var player in AnotherPlayer)
                total += map.GetScorePlayer(player);

            if (map.GetScorePlayer(myNumber) > total)
                // Если победил AI
                return 10000;
            else
                // Если победил человек
                return -10000;
        }

        // Устанавливается стадия игры
        if (level == 0)
        {
            float percentCell = (float)(map.Length * map.Width - map.CountEmptyCell) / (float)(map.Length * map.Width);
            SwitchGameSutuation(percentCell);
        }

        // Достигли максимальную глубину дерева
        if (level >= MAX_LEVEL)
            return EvaluationFunction(map);

        int MaxMinEvaluation = 0;

        // Обработка хода Бота
        if (level % 2 == 0)
        {
            // Получаем список всех ходов 
            avaible = GetAvaibleForPlayer(map, myNumber);
            MaxMinEvaluation = int.MinValue;

            for (int i = 0; i < avaible.Count; i++)
            {
                // Сколько процентов ходов обработать у фигуры
                float percentStep = figurePercent[(int)(map.Players[myNumber].figures[i].type) - 1];

                // Количество ходов которое будет обработано у фигуры
                int stepCalculate = (int)MathF.Round(avaible[i].Count * percentStep);

                // Если получилось что 0 шагов обработаются, то обработать хотя бы 1 шаг
                if ((stepCalculate < 1) && (avaible[i].Count >= 1))
                    stepCalculate = 1;

                for (int j = 0; j < stepCalculate; j++)
                {
                    if (stop || timer.Elapsed.TotalSeconds > TIMER) { stop = true; return MaxMinEvaluation; };

                    if (MaxMinEvaluation > beta) break;
                    if (beta < alpha) break;

                    Map copyMap = GameStateCalcSystem.ApplyStep(map, map.Players[myNumber].figures[i], avaible[i][j]);

                    int MinMax = AlphaBeta(copyMap, level + 1, alpha, beta);

                    // Запоминаем наилучший ход
                    if ((level == 0) && (MinMax > MaxMinEvaluation))
                    {
                        bestCell = avaible[i][j];
                        bestFigure = map.Players[myNumber].figures[i];
                    }

                    MaxMinEvaluation = Math.Max(MaxMinEvaluation, MinMax);
                    alpha = Math.Max(alpha, MaxMinEvaluation);
                }
            }
        }
        // Обработка хода Противника
        else
        {
            foreach (var player in AnotherPlayer)
            {
                avaible = GetAvaibleForPlayer(map, player);
                MaxMinEvaluation = int.MaxValue;

                for (int i = 0; i < avaible.Count; i++)
                {
                    float percentStep = figurePercent[(int)(map.Players[player].figures[i].type) - 1];
                    int stepCalculate = (int)MathF.Round(avaible[i].Count * percentStep);

                    if ((stepCalculate < 1) && (avaible[i].Count >= 1)) stepCalculate = 1;

                    for (int j = 0; j < stepCalculate; j++)
                    {
                        if (stop || timer.Elapsed.TotalSeconds > TIMER) { stop = true; return MaxMinEvaluation; };

                        if (MaxMinEvaluation < alpha) break;
                        if (beta < alpha) break;

                        Map copyMap = GameStateCalcSystem.ApplyStep(map, map.Players[player].figures[i], avaible[i][j]);

                        int MinMax = AlphaBeta(copyMap, level + 1, alpha, beta);

                        MaxMinEvaluation = Math.Min(MaxMinEvaluation, MinMax);
                        beta = Math.Min(beta, MaxMinEvaluation);
                    }
                }
            }
        }

        return MaxMinEvaluation;
    }


    private int EvaluationFunction(Map map)
    {
        // Оценочная функция
        var score = map.Score;

        int evaluation = 0;

        evaluation += score[myNumber][CellType.Paint] * pricePaint;
        evaluation += score[myNumber][CellType.Dark] * priceDark;

        foreach (var player in AnotherPlayer)
        {
            evaluation -= score[player][CellType.Paint] * pricePaint;
            evaluation -= score[player][CellType.Dark] * priceDark;

            // Если Рядом с Пешками Врага стоит фигура Бота начисляется штраф
            foreach (var figure in map.Players[player].figures)
                if (figure.type == FigureType.Pawn)
                    evaluation -= (Check.BesideEnemy(figure.pos, map, figure.Number)) ? priceAroundEnemyPawn : 0;
        }

        int total = 0;

        foreach (var player in AnotherPlayer)
            total += map.GetPlayerFiguresCount(player);

        // Разница в количестве живых фигур у игрока
        evaluation += (map.GetPlayerFiguresCount(myNumber) - total) * priceKill;

        return evaluation;
    }


    public Step getStep(Map CurrentGameState) 
    {
        stop = false;
        timer.Reset();
        myNumber = CurrentGameState.NumberPlayerStep;
        playerCount = CurrentGameState.PlayersCount;
        AnotherPlayer = new();

        for (int i = 0; i < playerCount; i++)
            if (i != myNumber) AnotherPlayer.Add(i);      
        
        timer.Start();

        AlphaBeta(CurrentGameState, 0, int.MinValue, int.MaxValue);

        return new Step(bestFigure, bestCell);
    }
}

