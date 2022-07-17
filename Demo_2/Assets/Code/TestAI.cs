using ColorChessModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class TestAI
{
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
	

	static int MAX_LEVEL = 2;
	public static int AlphaBeta(Map map, int level, int alpha, int beta)
	{
		List<List<ColorChessModel.Cell>> avaible = new List<List<ColorChessModel.Cell>>();
		int MaxMinEvaluation = 0;
		ColorChessModel.Cell bestCell = new ColorChessModel.Cell(new Position(-1, -1));
		ColorChessModel.Figure bestFigure = new ColorChessModel.Figure();

		//int statusGame = 0;
		//if (statusGame == 1) { return 10000;  }
		//else if (statusGame == 2) { return -10000; }

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
				if (beta < alpha) break;

				for (int j = 0; j < avaible[i].Count; j++)
				{
					if (MaxMinEvaluation > beta) break;
					if (beta < alpha) break;


					Map copyMap = GameStateCalcSystem.ApplyStep(map, map.players[1].figures[i], avaible[i][j]);


					int MinMax = AlphaBeta(copyMap, level + 1, alpha, beta);

					if (level == 0 && MinMax > MaxMinEvaluation)
					{
						bestCell = avaible[i][j];
						bestFigure = map.players[1].figures[i];
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
				if (beta < alpha) break;

				for (int j = 0; j < avaible[i].Count; j++)
				{
					if (MaxMinEvaluation < alpha) break;
					if (beta < alpha) break;

					Map copyMap = GameStateCalcSystem.ApplyStep(map, map.players[0].figures[i], avaible[i][j]);

					int MinMax = AlphaBeta(copyMap, level + 1, alpha, beta);

					MaxMinEvaluation = Math.Min(MaxMinEvaluation, MinMax);
					beta = Math.Min(alpha, MaxMinEvaluation);
				}
			}
		}

		if (level == 0)
		{
			Console.WriteLine("Самый лучший ход:" + bestCell);
			Console.WriteLine("Figure:" + bestFigure);
			bestCell1 = bestCell;
			bestFigure1 = bestFigure;
		}


		return MaxMinEvaluation;
	}

	public static ColorChessModel.Cell bestCell1 = null;
	public static ColorChessModel.Figure bestFigure1 = null;


	public static int evaluation_function(Map map)
	{
		return -map.scorePlayer[0] + map.scorePlayer[1];
	}

}
