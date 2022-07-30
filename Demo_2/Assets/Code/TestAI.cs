using ColorChessModel;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public  class TestAI
{
	// Глубина дерева
	static int MAX_LEVEL = 4;

	// Лучший ход
	public static Cell bestCell = null;
	public static Figure bestFigure = null;

	#region Параметры оценочной функции
	static int pricePaint = 9;
	static int priceDark = 10;
	static int priceKill = 70;
	static int priceAroundEnemyPawn = 60;

    #endregion

    #region Надо бы как-то это переименовать...
    static float pawnPercent = 0;
	static float castlePercent = 0;
	static float bishopWeight = 0;
	static float kingPercent = 0;
	static float horsePercent = 0;
	static float queenPercent = 0;

	private static void SetWeightStartGame()
    {
        //Начало
        pawnPercent = 1f;
        castlePercent = 0.5f;
        kingPercent = 0.1f;
        queenPercent = 0.4f;
        bishopWeight = 0.4f;
        horsePercent = 1f;

  //      pawnPercent = 1f;
		//castlePercent = 1f;
		//kingPercent = 0.1f;
		//queenPercent = 0.4f;
		//bishopWeight = 1f;
		//horsePercent = 1f;
	}

	private static void SetWeightSpeedGame()
	{
		// Разгон
		pawnPercent = 0.5f;
		castlePercent = 0.4f;
		kingPercent = 0.1f;
		queenPercent = 0.3f;
		bishopWeight = 0.3f;
		horsePercent = 1f;
	}

	private static void SetWeightMidleGame()
	{
		// Середина
		pawnPercent = 0.4f;
		castlePercent = 0.4f;
		kingPercent = 0.3f;
		queenPercent = 0.2f;
		bishopWeight = 0.2f;
		horsePercent = 0.7f;
	}

	private static void SetWeightSlowGame()
	{
		// Замедление
		pawnPercent = 1f;
		castlePercent = 0.4f;
		kingPercent = 0.4f;
		queenPercent = 0.2f;
		bishopWeight = 0.3f;
		horsePercent = 0.5f;
	}

	private static void SetWeightEndGame()
	{
		// Конец
		pawnPercent = 0.3f;
		castlePercent = 0.3f;
		kingPercent = 0.5f;
		queenPercent = 0.2f;
		bishopWeight = 0.2f;
		horsePercent = 0.3f;
	}
    #endregion

    private static List<List<Cell>> GetAvaibleForPlayer(Map map, int numberPlayer)
	{
		// Возвращает двумерный массив: первый индекс - i-ая фигура игрока; 
		// второй индекс - j-ая клетка на которую может сходить фигура

		List<List<Cell>> avaiblePlayer = new List<List<Cell>>();
		
		Player player = map.Players[numberPlayer];

		foreach (Figure figure in player.figures)
		{
			avaiblePlayer.Add(WayCalcSystem.CalcAllSteps(map, figure));
		}

		return avaiblePlayer;
	}

	private static void SwitchGameSutuation(float percentEmptyCell)
    {
		// Меняет стадию игры Начало-Разгон-Середина-Замедление-Конец
		if (percentEmptyCell < 0.25)
        {
			SetWeightStartGame();
		}
		else if (percentEmptyCell < 0.4)
        {
			SetWeightSpeedGame();
		}
		else if (percentEmptyCell < 0.7)
		{
			SetWeightMidleGame();
		}
		else if (percentEmptyCell < 0.9)
		{
			SetWeightSlowGame();
		}
		else if (percentEmptyCell < 1)
		{
			SetWeightEndGame();
		}
	}

	private static float GetFigurePercentForStep(FigureType figureType) 
	{
		// Возвращает сколько процентов ходов обработать у фигуры

        switch (figureType)
        {
            case FigureType.Empty:
				return 0;
            case FigureType.Pawn:
				return pawnPercent;
            case FigureType.King:
				return kingPercent;
            case FigureType.Bishop:
				return bishopWeight;
            case FigureType.Castle:
				return castlePercent;
            case FigureType.Horse:
				return horsePercent;
            case FigureType.Queen:
				return queenPercent;
            default:
				return 0;
        }
    }

	public static int AlphaBeta(Map map, int level, int alpha, int beta)
	{
		// Список всех возможных ходов для определенного игрока
		List<List<ColorChessModel.Cell>> avaible = new List<List<ColorChessModel.Cell>>();

		int MaxMinEvaluation = 0;

		// Конец игры
		if (map.EndGame == true)
		{
			if (map.GetScorePlayer(1) > map.GetScorePlayer(0))
			{
				// Если победил AI
				return 10000;
			}
			else
			{
				// Если победил человек
				return -10000;
			}
		}

		// Устанавливается стадия игры
		if (level == 0)
        {
			float percentCell = (float) (map.Length * map.Width - map.CountEmptyCell) / (float)(map.Length * map.Width);
			SwitchGameSutuation(percentCell);
        }

		// Достигли максимальную глубину дерева
		if (level >= MAX_LEVEL)
		{
			return EvaluationFunction(map);
		}

		// Обработка хода Бота
		if (level % 2 == 0)
		{
			// Получаем список всех ходов 
			avaible = GetAvaibleForPlayer(map, 1);
			MaxMinEvaluation = int.MinValue;


			for (int i = 0; i < avaible.Count; i++)
			{
				// Сколько процентов ходов обработать у фигуры
				float percentStep = GetFigurePercentForStep(map.Players[1].figures[i].type);

				// Количество ходов которое будет обработано у фигуры
				int stepCalculate = (int)MathF.Round(avaible[i].Count * percentStep);

				// Если получилось что 0 шагов обработаются, то обработать хотя бы 1 шаг
				if ((stepCalculate < 1) && (avaible[i].Count >= 1))
				{
					stepCalculate = 1;
				}

				for (int j = 0; j < stepCalculate; j++)
				{
					if (MaxMinEvaluation > beta) break;
					if (beta < alpha) break;

					Map copyMap = GameStateCalcSystem.ApplyStep(map, map.Players[1].figures[i], avaible[i][j]);

                    int MinMax = AlphaBeta(copyMap, level + 1, alpha, beta);

					// Запоминаем наилучший ход
					if ((level == 0) && (MinMax > MaxMinEvaluation))
					{
						bestCell = avaible[i][j];
						bestFigure = map.Players[1].figures[i];
					}

					MaxMinEvaluation = Math.Max(MaxMinEvaluation, MinMax);
					alpha = Math.Max(alpha, MaxMinEvaluation);
				}
			}
		}
		// Обработка хода Человека
		else
		{
			avaible = GetAvaibleForPlayer(map, 0);
			MaxMinEvaluation = int.MaxValue;

			for (int i = 0; i < avaible.Count; i++)
			{
				float percentStep = GetFigurePercentForStep(map.Players[0].figures[i].type);
				int stepCalculate = (int)MathF.Round(avaible[i].Count * percentStep);

				if ((stepCalculate < 1) && (avaible[i].Count >= 1))
				{
					stepCalculate = 1;
				}

				for (int j = 0; j < stepCalculate; j++)
				{
					if (MaxMinEvaluation < alpha) break;
					if (beta < alpha) break;

					Map copyMap = GameStateCalcSystem.ApplyStep(map, map.Players[0].figures[i], avaible[i][j]);

					int MinMax = AlphaBeta(copyMap, level + 1, alpha, beta);
                    MaxMinEvaluation = Math.Min(MaxMinEvaluation, MinMax);
					beta = Math.Min(beta, MaxMinEvaluation);
				}
			}
		}


        return MaxMinEvaluation;
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
        {
            if (figure.type == FigureType.Pawn)
            {
                evaluation -= (Check.BesideEnemy(figure.pos, map, figure.Number)) ? priceAroundEnemyPawn : 0;
            }
        }

        return evaluation;
	}

	public static void testStep(Map map, GameController _gameController)
    {
        Task t3 = Task.Run(() =>
        {
            DebugConsole.Print("Запуск Альфа Беты");
            AlphaBeta(map, 0, int.MinValue, int.MaxValue);
            DebugConsole.Print("Отправляю всё в гейм контроллер");
            _gameController.TestAIStepTest();
        });


		//testMap = map;

		//Thread myThread = new Thread(func);
		//myThread.Start();
	}

	static Map testMap;
	private static void func()
    {
		AlphaBeta(testMap, 0, int.MinValue, int.MaxValue);
	}


}

