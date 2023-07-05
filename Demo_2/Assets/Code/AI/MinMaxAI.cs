using ColorChessModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;

public class MinMaxAI : IAI
{
    // Глубина дерева
    int MAX_LEVEL = 4;

    // Лучший ход
    public Cell bestCell = null;
    public Figure bestFigure = null;

    // Параметры ОФ
    int pricePaint = 9;
    int priceDark = 10;
    int priceAroundEnemyPawn = 60;

    // pawn - castle - bishop - king - horse - queen 
    int[] priceFigure = { 80, 100, 60, 150, 75, 70};

    static float priceMiddle = 1.5f;
    static float priceCorner = 1.1f;
    static float priceGrawe = 1.3f;
    float[,] pricePos = { 
        { priceCorner,  priceGrawe,     priceCorner},
        { priceGrawe,   priceMiddle,    priceGrawe},
        { priceCorner,  priceGrawe,     priceCorner}
    };
    
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

    public MinMaxAI() 
    {
        //pricePaint = 0;
        //priceDark = 0;
        //priceAroundEnemyPawn = 0;

        //// pawn - castle - bishop - king - horse - queen 
        ////priceFigure
        ////figurePercentStart
        ////figurePercentSpeed
        ////figurePercentMidle
        ////figurePercentSlow
        ////figurePercentEnd

        //for (int i = 0; i < priceFigure.Length; i++) 
        //{
        //    priceFigure[i] = 0;
        //}


        //priceMiddle = 0;
        //priceCorner = 0;
        //priceGrawe = 0;
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
        float evaluation = 0;

        // Если Рядом с Пешками Врага стоит фигура Бота начисляется штраф
        foreach (var player in AnotherPlayer)
            foreach (var figure in map.Players[player].figures) 
                if ((figure.type == FigureType.Pawn) && (Check.BesideEnemy(figure.pos, map, myNumber)))
                        evaluation -= priceAroundEnemyPawn;

        for (int i = 0; i < map.Length; i++)
        {
            for (int j = 0; j < map.Width; j++)
            {
                Cell cell = map.Cells[i, j];
                if (cell.numberPlayer == -1) continue;

                float _pricePos = pricePos[(i % 9) / 3, (j % 9) / 3];
                float priceCell = 0;


                if (cell.FigureType != FigureType.Empty)
                    priceCell += (priceFigure[(int)cell.FigureType - 1] * _pricePos);

                if (cell.type == CellType.Paint) priceCell += (pricePaint * _pricePos);
                else if (cell.type == CellType.Dark) priceCell += (priceDark * _pricePos);

                
                if (cell.numberPlayer == myNumber) 
                    evaluation += priceCell;
                else
                    evaluation -= priceCell;
            }
        }

        return (int)evaluation;
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

