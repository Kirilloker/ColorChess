using ColorChessModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

public class MinMaxAI : IAI
{
    // Глубина дерева
    int MAX_LEVEL = 4;

    // Лучший ход
    public Cell bestCell = null;
    public Figure bestFigure = null;

    // Параметры ОФ
    float pricePaint = 9;
    float priceDark = 10;
    //float priceAroundEnemyPawn = 100;


    // pawn - castle - bishop - king - horse - queen 
    float[] priceFigure = { 220, 400, 200, 290, 205, 250};

    static float priceMiddle = 1.5f;
    static float priceCorner = 1.1f;
    static float priceGrave = 1.3f;
    float[,] pricePos = { 
        { priceCorner,  priceGrave,     priceCorner},
        { priceGrave,   priceMiddle,    priceGrave},
        { priceCorner,  priceGrave,     priceCorner}
    };
    
    //                                pawn      castle  bishop king     horse   queen 
    float[] figurePercent =         { 0,        0,      0,      0,      0,      0 };
    float[] figurePercentStart =    { 0.8f,     0.5f,   0.1f,   0.2f,   0.4f,   0.8f };
    float[] figurePercentSpeed =    { 0.5f,     0.4f,   0.1f,   0.2f,   0.3f,   0.5f };
    float[] figurePercentMiddle =   { 0.4f,     0.4f,   0.3f,   0.2f,   0.2f,   0.5f };
    float[] figurePercentSlow =     { 0.8f,     0.4f,   0.4f,   0.2f,   0.3f,   0.4f };
    float[] figurePercentEnd =      { 0.6f,     0.3f,   0.5f,   0.7f,   0.2f,   0.2f };

    // Номер бота
    int myNumber = 0;
    // Количество человек в игре 
    int playerCount = 0;
    List<int> AnotherPlayer;

    // Таймер, который следит за временем выполнение Alpha-Beta
    Stopwatch timer = new();
    // Время, выделенное на расчет Alpha-Beta
    float TIMER;
    // Вспомогательная переменная, которая останавливает расчет в случае долгого выполнения
    bool stop = false;

    private static readonly Random random = new Random();
    public static float GenerateRandomValue(float a, float range = 2f)
    {
        // Определяем границы для нормального распределения
        float min = a / range;
        float max = a * range;

        // Генерируем случайное значение по нормальному распределению
        float randomValue = (float)(random.NextDouble() * (max - min) + min);

        // Вычисляем вес для приближения к заданному значению A
        float weight = Math.Max(0f, Math.Min(1f, Math.Abs(randomValue - a) / (max - min)));

        // Возвращаем случайное значение с учетом веса
        return a + (randomValue - a) * weight;
    }


    public MinMaxAI() 
    {
        pricePaint = GenerateRandomValue(pricePaint);
        priceDark = GenerateRandomValue(priceDark);
        //priceAroundEnemyPawn = GenerateRandomValue(priceAroundEnemyPawn);

        for (int i = 0; i < figurePercentStart.Length; i++)
        {
            figurePercentStart[i] = GenerateRandomValue(figurePercentStart[i], 1.3f);
            if (figurePercentStart[i] > 1f) figurePercentStart[i] = 1f;
            else if (figurePercentStart[i] < 0f) figurePercentStart[i] = 0f;

            figurePercentSpeed[i] = GenerateRandomValue(figurePercentSpeed[i], 1.3f);
            if (figurePercentSpeed[i] > 1f) figurePercentSpeed[i] = 1f;
            else if (figurePercentSpeed[i] < 0f) figurePercentSpeed[i] = 0f;

            figurePercentMiddle[i] = GenerateRandomValue(figurePercentMiddle[i], 1.3f);
            if (figurePercentMiddle[i] > 1f) figurePercentMiddle[i] = 1f;
            else if (figurePercentMiddle[i] < 0f) figurePercentMiddle[i] = 0f;

            figurePercentSlow[i] = GenerateRandomValue(figurePercentSlow[i], 1.3f);
            if (figurePercentSlow[i] > 1f) figurePercentSlow[i] = 1f;
            else if (figurePercentSlow[i] < 0f) figurePercentSlow[i] = 0f;

            figurePercentEnd[i] = GenerateRandomValue(figurePercentEnd[i], 1.3f);
            if (figurePercentEnd[i] > 1f) figurePercentEnd[i] = 1f;
            else if (figurePercentEnd[i] < 0f) figurePercentEnd[i] = 0f;
        }


        for (int i = 0; i < priceFigure.Length; i++)
            priceFigure[i] = GenerateRandomValue(priceFigure[i]);


        priceMiddle = GenerateRandomValue(priceMiddle, 1.5f);
        priceCorner = GenerateRandomValue(priceCorner, 1.5f);
        priceGrave = GenerateRandomValue(priceGrave, 1.5f);

        if (priceMiddle < 0.9f) priceMiddle = 0.9f;
        if (priceCorner < 0.9f) priceCorner = 0.9f;
        if (priceGrave < 0.9f) priceGrave = 0.9f;
    }

    private List<List<Cell>> GetAvailableForPlayer(Map map, int numberPlayer)
    {
        // Возвращает двумерный массив: первый индекс - i-ая фигура игрока; 
        // второй индекс - j-ая клетка на которую может сходить фигура

        List<List<Cell>> availablePlayer = new();

        Player player = map.Players[numberPlayer];

        foreach (Figure figure in player.Figures)
            availablePlayer.Add(WayCalcSystem.CalcAllSteps(map, figure));

        return availablePlayer;
    }

    private void SwitchGameSituation(float percentEmptyCell)
    {
        // Меняет стадию игры Начало-Разгон-Середина-Замедление-Конец
        if (percentEmptyCell < 0.25)
            figurePercent = figurePercentStart;
        else if (percentEmptyCell < 0.4)
            figurePercent = figurePercentSpeed;
        else if (percentEmptyCell < 0.7)
            figurePercent = figurePercentMiddle;
        else if (percentEmptyCell < 0.9)
            figurePercent = figurePercentSlow;
        else if (percentEmptyCell < 1)
            figurePercent = figurePercentEnd;
    }

    private int AlphaBeta(Map map, int level, int alpha, int beta)
    {
        int MaxMinEvaluation = 0;

        // Список всех возможных ходов для определенного игрока
        List<List<Cell>> available = new();

        // Конец игры
        if (map.EndGame == true)
        {
            int total = 0;

            foreach (var player in AnotherPlayer)
                total += map.GetScorePlayer(player);

            if (map.GetScorePlayer(myNumber) > total)
                // Если победил AI
                return int.MaxValue;
            else
                // Если победил человек
                return - Math.Abs(beta) * 3;
        }

        // Устанавливается стадия игры
        if (level == 0)
        {
            float percentCell = (float)(map.Length * map.Width - map.CountEmptyCell) / (float)(map.Length * map.Width);
            SwitchGameSituation(percentCell);
        }

        // Достигли максимальную глубину дерева
        if (level >= MAX_LEVEL)
            return EvaluationFunction(map);


        // Обработка хода Бота
        if (level % 2 == 0)
        {
            // Получаем список всех ходов 
            available = GetAvailableForPlayer(map, myNumber);
            MaxMinEvaluation = int.MinValue;

            for (int i = 0; i < available.Count; i++)
            {
                // Сколько процентов ходов обработать у фигуры
                float percentStep = figurePercent[(int)(map.Players[myNumber].Figures[i].Type) - 1];

                // Количество ходов которое будет обработано у фигуры
                int stepCalculate = (int)Math.Round(available[i].Count * percentStep);

                // Если получилось что 0 шагов обработаются, то обработать хотя бы 1 шаг
                if ((stepCalculate < 1) && (available[i].Count >= 1))
                    stepCalculate = 1;

                for (int j = 0; j < stepCalculate; j++)
                {
                    if (stop || timer.Elapsed.TotalSeconds > TIMER) { stop = true; return MaxMinEvaluation; };

                    if (MaxMinEvaluation > beta) break;
                    if (beta < alpha) break;


                    Map copyMap = GameStateCalcSystem.ApplyStep(map, map.Players[myNumber].Figures[i], available[i][j]);

                    int MinMax = AlphaBeta(copyMap, level + 1, alpha, beta);

                    // Запоминаем наилучший ход
                    if ((level == 0) && (MinMax > MaxMinEvaluation))
                    {
                        bestCell = available[i][j];
                        bestFigure = map.Players[myNumber].Figures[i];
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
                available = GetAvailableForPlayer(map, player);
                MaxMinEvaluation = int.MaxValue;

                for (int i = 0; i < available.Count; i++)
                {
                    float percentStep = figurePercent[(int)(map.Players[player].Figures[i].Type) - 1];
                    int stepCalculate = (int)Math.Round(available[i].Count * percentStep);

                    if ((stepCalculate < 1) && (available[i].Count >= 1)) stepCalculate = 1;

                    for (int j = 0; j < stepCalculate; j++)
                    {
                        if (stop || timer.Elapsed.TotalSeconds > TIMER) { stop = true; return MaxMinEvaluation; };

                        if (MaxMinEvaluation < alpha) break;
                        if (beta < alpha) break;

                        Map copyMap = GameStateCalcSystem.ApplyStep(map, map.Players[player].Figures[i], available[i][j]);

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
        float evaluation = 0;

        // Если Рядом с Пешками Врага стоит фигура Бота начисляется штраф
        //foreach (var player in AnotherPlayer)
        //    foreach (var figure in map.Players[player].Figures) 
        //        if ((figure.Type == FigureType.Pawn) && (Check.BesideEnemy(figure.Pos, map, myNumber)))
        //                evaluation -= priceAroundEnemyPawn;

        for (int i = 0; i < map.Length; i++)
        {
            for (int j = 0; j < map.Width; j++)
            {
                Cell cell = map.Cells[i, j];
                if (cell.NumberPlayer == -1) continue;

                float _pricePos = pricePos[(i % 9) / 3, (j % 9) / 3];
                float priceCell = 0;


                if (cell.FigureType != FigureType.Empty)
                    priceCell += (priceFigure[(int)cell.FigureType - 1] * _pricePos);

                if (cell.Type == CellType.Paint) priceCell += (pricePaint * _pricePos);
                else if (cell.Type == CellType.Dark) priceCell += (priceDark * _pricePos);

                
                if (cell.NumberPlayer == myNumber) 
                    evaluation += priceCell;
                else
                    evaluation -= priceCell;
            }
        }

        return (int)evaluation;
    }


    public Step getStep(Map CurrentGameState) 
    {
        bestFigure = null;
        bestCell = null;

        stop = false;
        timer.Reset();
        myNumber = CurrentGameState.NumberPlayerStep;
        playerCount = CurrentGameState.PlayersCount;
        AnotherPlayer = new();
        TIMER = 8f + (playerCount - 2) * 3f;

        for (int i = 0; i < playerCount; i++)
            if (i != myNumber) AnotherPlayer.Add(i);

        timer.Start();

        AlphaBeta(CurrentGameState, 0, int.MinValue, int.MaxValue);

        if (bestFigure == null) 
        {
            int numberFigure = 0;
            while (bestCell == null) 
            {
                bestFigure = CurrentGameState.Players[myNumber].Figures[numberFigure];
                bestCell = WayCalcSystem.CalcAllSteps(CurrentGameState, bestFigure)[0];

                if (CurrentGameState.Players[myNumber].Figures.Count < numberFigure - 1)
                    break;

                numberFigure++;
            }
        }

        if (bestCell == null)
            throw new Exception("Not found any step");

        return new Step(bestFigure, bestCell);
    }
}

