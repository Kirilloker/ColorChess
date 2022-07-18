using ColorChessModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class TestAI
{
	static float pawnWeight = 0;
	static float castleWeight = 0;
	static float bishopWeight = 0;
	static float kingWeight = 0;
	static float horseWeight = 0;
	static float queenWeight = 0;

	private static void SetWeightStartGame()
    {
		// Начало
		pawnWeight = 1f;
        castleWeight = 0.5f;
        kingWeight = 0.1f;
        queenWeight = 0.4f;
        bishopWeight = 0.4f;
        horseWeight = 1f;
    }

	private static void SetWeightSpeedGame()
	{
		// Разгон
		pawnWeight = 0.5f;
		//castleWeight = 1f;
		castleWeight = 0.4f;
		kingWeight = 0.1f;
		queenWeight = 0.3f;
		bishopWeight = 0.3f;
		horseWeight = 1f;
	}

	private static void SetWeightMidleGame()
	{
		// Середина
		pawnWeight = 0.4f;
		castleWeight = 0.4f;
		kingWeight = 0.3f;
		queenWeight = 0.2f;
		bishopWeight = 0.2f;
		horseWeight = 0.7f;
	}

	private static void SetWeightSlowGame()
	{
		// Замедление
		pawnWeight = 1f;
		castleWeight = 0.4f;
		kingWeight = 0.4f;
		queenWeight = 0.2f;
		bishopWeight = 0.3f;
		horseWeight = 0.5f;
	}

	private static void SetWeightEndGame()
	{
		// Конец
		pawnWeight = 0.5f;
		castleWeight = 0.4f;
		kingWeight = 0.4f;
		queenWeight = 0.2f;
		bishopWeight = 0.2f;
		horseWeight = 0.5f;
	}


	// ИСПРАВИТЬ ВСЁ ТУТ
	public static List<List<ColorChessModel.Cell>> TestGetAvaible(Map map, int numberPlayer)
	{
		List<List<ColorChessModel.Cell>> avaiblePlayer = new List<List<ColorChessModel.Cell>>();
		
		ColorChessModel.Player player = map.players[numberPlayer];

		foreach (ColorChessModel.Figure figure in player.figures)
		{
			avaiblePlayer.Add(WayCalcSystem.CalcAllSteps(map, figure));
		}

		return avaiblePlayer;
	}

	// Написать функцию, которая обработает доску 
	// То есть - изменит состояние клеток и всякое такое
	
	public static void SwitchGameSutuation(float percentEmptyCell)
    {
		if (percentEmptyCell < 0.25)
        {
			//DebugConsole.Print("начальная стадия");
			SetWeightStartGame();
		}
		else if (percentEmptyCell < 0.4)
        {
			//DebugConsole.Print("разгон стадия");
			SetWeightSpeedGame();
		}
		else if (percentEmptyCell < 0.7)
		{
			//DebugConsole.Print("серединная стадия");
			SetWeightMidleGame();
		}
		else if (percentEmptyCell < 0.9)
		{
			//DebugConsole.Print("Замедление стадия");
			SetWeightSlowGame();
		}
		else if (percentEmptyCell < 1)
		{
			//DebugConsole.Print("Конечная стадия");
			SetWeightEndGame();
		}
	}

	public static float GetFigurePercentForStep(FigureType figureType) 
	{
        switch (figureType)
        {
            case FigureType.Empty:
				return 0;
                break;
            case FigureType.Pawn:
				return pawnWeight;
                break;
            case FigureType.King:
				return kingWeight;
				break;
            case FigureType.Bishop:
				return bishopWeight;
				break;
            case FigureType.Castle:
				return castleWeight;
				break;
            case FigureType.Horse:
				return horseWeight;
				break;
            case FigureType.Queen:
				return queenWeight;
				break;
            default:
				return 0;
				break;
        }
    }

	static int MAX_LEVEL = 4;
	public static int AlphaBeta(Map map, int level, int alpha, int beta)
	{
		List<List<ColorChessModel.Cell>> avaible = new List<List<ColorChessModel.Cell>>();

		int MaxMinEvaluation = 0;

		if (map.EndGame == true)
		{
			if (map.GetScorePlayer(1) > map.GetScorePlayer(0))
			{
				return 100000;
			}
			else
			{
				return -100000;
			}
		}

		if (level == 0)
        {
			// Устанавливается стадия игры

			float percentCell = (float) (map.Length * map.Width - map.CountEmptyCell) / (float)(map.Length * map.Width);

			//float percentCell = (float)map.CountEmptyCell / (map.Length * (float)map.Width);
			SwitchGameSutuation(percentCell);
        }


		if (level >= MAX_LEVEL)
		{
			return evaluation_function(map);
		}


		if (level % 2 == 0)
		{
			avaible = TestGetAvaible(map, 1);
			MaxMinEvaluation = int.MinValue;


			for (int i = 0; i < avaible.Count; i++)
			{
				//if (beta < alpha) break;

				FigureType figureType = map.players[1].figures[i].type;

				float percentStep = GetFigurePercentForStep(figureType);
				int stepCalculate = (int)MathF.Round(avaible[i].Count * percentStep);

				if ((stepCalculate < 1) && (avaible[i].Count >= 1))
				{
					stepCalculate = 1;
				}

				for (int j = 0; j < stepCalculate; j++)
				{
					if (MaxMinEvaluation > beta) break;
					if (beta < alpha) break;

					Map copyMap = GameStateCalcSystem.ApplyStep(map, map.players[1].figures[i], avaible[i][j]);

					int MinMax = AlphaBeta(copyMap, level + 1, alpha, beta);

					if ((level == 0) && (MinMax > MaxMinEvaluation))
					{;
						bestCell1 = avaible[i][j];
						bestFigure1 = map.players[1].figures[i];
					}

					MaxMinEvaluation = Math.Max(MaxMinEvaluation, MinMax);
					alpha = Math.Max(alpha, MaxMinEvaluation);
				}
			}
		}
		else
		{
			avaible = TestGetAvaible(map, 0);
			MaxMinEvaluation = int.MaxValue;

			for (int i = 0; i < avaible.Count; i++)
			{
				FigureType figureType = map.players[0].figures[i].type;

				float percentStep = GetFigurePercentForStep(figureType);
				int stepCalculate = (int)MathF.Round(avaible[i].Count * percentStep);

				if ((stepCalculate < 1) && (avaible[i].Count >= 1))
				{
					stepCalculate = 1;
				}

				for (int j = 0; j < stepCalculate; j++)
				{
					if (MaxMinEvaluation < alpha) break;
					if (beta < alpha) break;

					Map copyMap = GameStateCalcSystem.ApplyStep(map, map.players[0].figures[i], avaible[i][j]);

					int MinMax = AlphaBeta(copyMap, level + 1, alpha, beta);

					MaxMinEvaluation = Math.Min(MaxMinEvaluation, MinMax);
					beta = Math.Min(beta, MaxMinEvaluation);
				}
			}
		}



		return MaxMinEvaluation;
	}

	public static ColorChessModel.Cell bestCell1 = null;
	public static ColorChessModel.Figure bestFigure1 = null;


	public static int evaluation_function(Map map)
	{
		var score = map.Score;

		int evaluation = 0;

		evaluation += score[1][CellType.Paint] * 9;
		evaluation += score[1][CellType.Dark] * 10;

		evaluation -= score[0][CellType.Paint] * 9;
		evaluation -= score[0][CellType.Dark] * 10;


        evaluation += (map.players[1].figures.Count - map.players[0].figures.Count) * 70;

        foreach (var figure in map.players[0].figures)
        {
            if (figure.type == FigureType.Pawn)
            {
                evaluation -= (Check.BesideEnemy(figure.pos, map, figure.Number)) ? 60 : 0;
            }
        }

        return evaluation;
	}

    public static Dictionary<uint, int> maps = new Dictionary<uint, int>(100000);
    public static Dictionary<uint, Map> mapsTest = new Dictionary<uint, Map>(100000);

    public static uint TestInt = 0; 
}

