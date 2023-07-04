using ColorChessModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;

public class TestAI
{
    // Глубина дерева
    static int MAX_LEVEL = 6;

    // Лучший ход
    public static Cell bestCell = null;
    public static Figure bestFigure = null;

    // Параметры ОФ
    static int pricePaint = 9;
    static int priceDark = 10;
    static int priceKill = 70;
    static int priceAroundEnemyPawn = 60;

    // pawn - castle - bishop - king - horse - queen 
    static float[] figurePercent = { 0, 0, 0, 0, 0, 0 };
    static float[] figurePercentStart = { 1f, 0.5f, 0.1f, 0.4f, 0.4f, 1f };
    static float[] figurePercentSpeed = { 0.5f, 0.4f, 0.1f, 0.3f, 0.3f, 1f };
    static float[] figurePercentMidle = { 0.4f, 0.4f, 0.3f, 0.2f, 0.2f, 0.7f };
    static float[] figurePercentSlow = { 1f, 0.4f, 0.4f, 0.2f, 0.3f, 0.5f };
    static float[] figurePercentEnd = { 0.3f, 0.3f, 0.5f, 0.2f, 0.2f, 0.3f };


    private static List<List<Cell>> GetAvaibleForPlayer(Map map, int numberPlayer)
    {
        // Возвращает двумерный массив: первый индекс - i-ая фигура игрока; 
        // второй индекс - j-ая клетка на которую может сходить фигура

        List<List<Cell>> avaiblePlayer = new();

        Player player = map.Players[numberPlayer];

        foreach (Figure figure in player.figures)
            avaiblePlayer.Add(WayCalcSystem.CalcAllSteps(map, figure));

        return avaiblePlayer;
    }

    private static void SwitchGameSutuation(float percentEmptyCell)
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

    public static int AlphaBeta(Map map, int level, int alpha, int beta)
    {
        TestWatch.Start1();
        // Список всех возможных ходов для определенного игрока
        List<List<Cell>> avaible = new();

        // Конец игры
        if (map.EndGame == true)
        {
            if (map.GetScorePlayer(1) > map.GetScorePlayer(0))
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


        int MaxMinEvaluation;

        TestWatch.Stop1();

        // Обработка хода Бота
        if (level % 2 == 0)
        {
            // Получаем список всех ходов 
            TestWatch.Start2();
            avaible = GetAvaibleForPlayer(map, 1);
            MaxMinEvaluation = int.MinValue;
            TestWatch.Stop2();

            TestWatch.Start4();

            for (int i = 0; i < avaible.Count; i++)
            {
                TestWatch.Start3();
                // Сколько процентов ходов обработать у фигуры
                float percentStep = figurePercent[(int)(map.Players[1].figures[i].type) - 1];

                // Количество ходов которое будет обработано у фигуры
                //int stepCalculate = (int)MathF.Round(avaible[i].Count * percentStep);
                int stepCalculate = avaible[i].Count;
                stepCalculate = 2;
                if (avaible[i].Count <= stepCalculate) stepCalculate = avaible[i].Count;

                // Если получилось что 0 шагов обработаются, то обработать хотя бы 1 шаг
                if ((stepCalculate < 1) && (avaible[i].Count >= 1))
                    stepCalculate = 1;

                TestWatch.Stop3();

                TestWatch.Start6();

                for (int j = 0; j < stepCalculate; j++)
                {
                    TestWatch.Start5();
                    if (MaxMinEvaluation > beta) break;
                    if (beta < alpha) break;

                    Map copyMap = GameStateCalcSystem.ApplyStep(map, map.Players[1].figures[i], avaible[i][j]);
                    TestWatch.Stop5();

                    //TestWatch.Start6();
                    int MinMax = AlphaBeta(copyMap, level + 1, alpha, beta);
                    TestTrash.test++;

                    //var x = GameStateToIntArray.ConvertMapToIntArray(copyMap);
                    //int test = TestTrash.Contain(x);

                    //int MinMax;
                    //if (test != Int32.MinValue) MinMax = test;
                    //else
                    //{
                    //    MinMax = AlphaBeta(copyMap, level + 1, alpha, beta);
                    //    TestTrash.Add(x, MinMax);
                    //}


                    // Запоминаем наилучший ход
                    if ((level == 0) && (MinMax > MaxMinEvaluation))
                    {
                        bestCell = avaible[i][j];
                        bestFigure = map.Players[1].figures[i];
                    }

                    MaxMinEvaluation = Math.Max(MaxMinEvaluation, MinMax);
                    alpha = Math.Max(alpha, MaxMinEvaluation);
                }

                TestWatch.Stop6();
            }

            TestWatch.Stop4();
        }
        // Обработка хода Человека
        else
        {
            avaible = GetAvaibleForPlayer(map, 0);
            MaxMinEvaluation = int.MaxValue;

            for (int i = 0; i < avaible.Count; i++)
            {
                float percentStep = figurePercent[(int)(map.Players[0].figures[i].type) - 1];
                //int stepCalculate = (int)MathF.Round(avaible[i].Count * percentStep);
                int stepCalculate = avaible[i].Count;
                stepCalculate = 2;
                if (avaible[i].Count <= stepCalculate) stepCalculate = avaible[i].Count;

                if ((stepCalculate < 1) && (avaible[i].Count >= 1)) stepCalculate = 1;

                for (int j = 0; j < stepCalculate; j++)
                {
                    if (MaxMinEvaluation < alpha) break;
                    if (beta < alpha) break;

                    Map copyMap = GameStateCalcSystem.ApplyStep(map, map.Players[0].figures[i], avaible[i][j]);

                    int MinMax = AlphaBeta(copyMap, level + 1, alpha, beta);

                    TestTrash.test++;
                    //var x = GameStateToIntArray.ConvertMapToIntArray(copyMap);
                    //int test = TestTrash.Contain(x);


                    //int MinMax;
                    //if (test != Int32.MinValue) MinMax = test;
                    //else
                    //{
                    //    MinMax = AlphaBeta(copyMap, level + 1, alpha, beta);
                    //    TestTrash.Add(x, MinMax);
                    //}

                    MaxMinEvaluation = Math.Min(MaxMinEvaluation, MinMax);
                    beta = Math.Min(beta, MaxMinEvaluation);
                }
            }
        }

        return MaxMinEvaluation
;
    }


    public static int EvaluationFunction(Map map)
    {
        // Оценочная функция
        var score = map.Score;

        int evaluation = 0;

        evaluation += score[1][CellType.Paint] * pricePaint;
        evaluation += score[1][CellType.Dark] * priceDark;

        evaluation -= score[0][CellType.Paint] * pricePaint;
        evaluation -= score[0][CellType.Dark] * priceDark;

        // Разница в количестве живых фигур у игрока
        evaluation += (map.GetPlayerFiguresCount(1) - map.GetPlayerFiguresCount(0)) * priceKill;

        // Если Рядом с Пешками Врага стоит фигура Бота начисляется штраф
        foreach (var figure in map.Players[0].figures)
            if (figure.type == FigureType.Pawn)
                evaluation -= (Check.BesideEnemy(figure.pos, map, figure.Number)) ? priceAroundEnemyPawn : 0;

        return evaluation;
    }


}

