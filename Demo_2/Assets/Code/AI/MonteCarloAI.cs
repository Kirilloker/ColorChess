using ColorChessModel;
using System;
using System.Collections.Generic;

public class MonteCarloAI : IAI
{
    private static int numOfRandomGames = 5000;
    private static int randomGameDepth = 15;

    public Step getStep(Map startGameState)
    {
        if (startGameState.EndGame) throw new Exception("Try to GetBestStep in MonteCarloAI when game already ended");
        Random rnd = new Random();
        List<List<Cell>> avaibleFirstSteps = GetAvaibleStepsForActivePlayer(startGameState);
        //Ключ в паре - количество игр, значение - суммарная оценка всех игр.
        List<List<Pair<int, int>>> stepsCountAndScore = new List<List<Pair<int, int>>>(avaibleFirstSteps.Count);
        for (int i = 0; i < avaibleFirstSteps.Count; i++)
        {
            stepsCountAndScore.Add(new List<Pair<int, int>>(avaibleFirstSteps[i].Count));
            for (int j = 0; j < avaibleFirstSteps[i].Count; j++)
            {
                stepsCountAndScore[i].Add(new Pair<int, int> (0, 0));
            }
        }

        for (int i = 0; i < numOfRandomGames; i++)
        {
            Map startGameStateCopy = new Map(startGameState);
            int figureNum = rnd.Next(avaibleFirstSteps.Count);
            int cellNum = rnd.Next(avaibleFirstSteps[figureNum].Count);
            
            while (avaibleFirstSteps[figureNum].Count == 0)
            {
                figureNum = rnd.Next(avaibleFirstSteps.Count);
                cellNum = rnd.Next(avaibleFirstSteps[figureNum].Count);
            }

            Figure figureForStep = startGameStateCopy.Players[startGameStateCopy.NumberPlayerStep].figures[figureNum];
            Cell cellForStep = avaibleFirstSteps[figureNum][cellNum];
            startGameStateCopy = GameStateCalcSystem.ApplyStep(startGameStateCopy, figureForStep, cellForStep);
            Map randomGameResult = MakeNRandomSteps(randomGameDepth - 1, startGameState);
            int gameStateScore = EvaluationFunction(randomGameResult);
            stepsCountAndScore[figureNum][cellNum].First++;
            stepsCountAndScore[figureNum][cellNum].Second += gameStateScore;
        }

        int fig = 0, cell = 0, maxAverage = int.MinValue, curAverage = 0;

        for (int i = 0; i < stepsCountAndScore.Count; i++)
        {
            for (int j = 0; j < stepsCountAndScore[i].Count; j++)
            {
                if (stepsCountAndScore[i][j].First == 0) continue;
                curAverage = stepsCountAndScore[i][j].Second / stepsCountAndScore[i][j].First;
                if (curAverage > maxAverage)
                {
                    maxAverage = curAverage;
                    fig = i;
                    cell = j;
                }
            }
        }

        UnityEngine.Debug.Log(maxAverage);
        return new Step(startGameState.Players[startGameState.NumberPlayerStep].figures[fig], avaibleFirstSteps[fig][cell]);
    }

    private static Map MakeNRandomSteps(int numOfSteps, Map startGameState)
    {
        if (startGameState.EndGame) return startGameState;
        Random rnd = new Random();
        Map gameState = new Map(startGameState);
        
        for (int i = 0; i < numOfSteps; i++)
        {
            if (gameState.EndGame) return gameState;
            List<List<Cell>> avaibleSteps = GetAvaibleStepsForActivePlayer(gameState);
            int figureNum = rnd.Next(avaibleSteps.Count);
            int cellNum = rnd.Next(avaibleSteps[figureNum].Count);

            while (avaibleSteps[figureNum].Count == 0) 
            {
                figureNum = rnd.Next(avaibleSteps.Count);
                cellNum = rnd.Next(avaibleSteps[figureNum].Count);
            }

            Figure figureForStep = gameState.Players[gameState.NumberPlayerStep].figures[figureNum];
            Cell cellForStep = avaibleSteps[figureNum][cellNum];
            gameState = GameStateCalcSystem.ApplyStep(gameState, figureForStep, cellForStep);
        }

        return gameState; 
    }

    private static List<List<Cell>> GetAvaibleStepsForActivePlayer(Map gameState)
    {
        // Возвращает двумерный массив: первый индекс - i-ая фигура игрока 
        // второй индекс - j-ая клетка на которую может сходить фигура
       
        List<List<Cell>> avaibleSteps = new();
        int curPlayerNumber = gameState.NumberPlayerStep;
        List<Figure> figures = gameState.Players[curPlayerNumber].figures;

        foreach (Figure figure in figures)
            avaibleSteps.Add(WayCalcSystem.CalcAllSteps(gameState, figure));

        return avaibleSteps;
    }

    private static int EvaluationFunction(Map map)
    {
        // Параметры ОФ
        int pricePaint = 9;
        int priceDark = 10;
        int priceKill = 70;
        int priceAroundEnemyPawn = 60;

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

public class Pair<T, U>
{
    public Pair()
    {
    }

    public Pair(T first, U second)
    {
        this.First = first;
        this.Second = second;
    }

    public T First { get; set; }
    public U Second { get; set; }
};

