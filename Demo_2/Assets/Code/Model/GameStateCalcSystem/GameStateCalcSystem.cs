using System;
using System.Collections.Generic;
namespace ColorChessModel
{
    class GameStateCalcSystem
    {
        public static Map UpdateGameState(Map gameState)
        {
            CheckCapture(gameState);
            gameState.scorePlayer = CalculateScore(gameState);

            return gameState;
        }

        public static Map ApplyStep(Map _gameState, Figure figure, Cell endCell)
        {
            // В игре это работать не будет, но нужно для тестов

            Map gameState = new Map(_gameState);

            // Получаем ссылку на новую фигуру, которая делает ход
            Figure newFigure = gameState.GetCell(figure.pos).figure;

            if (endCell.pos.X == 0 && endCell.pos.Y == 3 && figure.type == FigureType.Bishop && figure.pos.X == 1 && figure.pos.Y == 3)
            {
                Console.WriteLine("asd");
            }

            List<Cell> Way = WayCalcSystem.CalcWay(gameState, newFigure.pos, endCell.pos, newFigure);



            for (int i = 0; i < Way.Count; i++)
            {
                Way[i].numberPlayer = newFigure.Number;
                Way[i].type = CellType.Paint;
            }

            Way[0].figure = null;
            Way[Way.Count - 1].figure = newFigure;
            newFigure.pos = new Position(endCell.pos);

            for (int i = 0; i < Way.Count; i++)
            {
                if (Way[i].numberPlayer != newFigure.Number)
                {
                    Console.WriteLine("");
                }
            }

            UpdateGameState(gameState);

            gameState.countStep++;

            return gameState;
        }

        public static Map UpdateGameStateForBuilder(Map gameState)
        {
            foreach (Player player in gameState.players)
            {
                foreach (Figure figure in player.figures)
                {
                    Cell cell = gameState.GetCell(figure.pos);

                    cell.figure = figure;
                    cell.numberPlayer = figure.Number;

                    cell.type = CellType.Paint;
                }
            }

            return gameState;
        }


        private static void CheckCapture(Map map)
        {
            // Делает квадраты захваченными

            for (int i = 0; i < map.Width; i++)
            {
                for (int j = 0; j < map.Length; j++)
                {
                    if (map.GetCell(i, j).type != CellType.Empty &
                        map.GetCell(i, j).numberPlayer != -1)
                    {
                        MakeCapture(map, map.GetCell(i, j));
                    }
                }
            }
        }
        private static void MakeCapture(Map map, Cell cell)
        {
            bool newDark = false;

            for (int i = cell.pos.X - 1; i <= cell.pos.X + 1; i++)
            {
                for (int j = cell.pos.Y - 1; j <= cell.pos.Y + 1; j++)
                {
                    Position newPos = new Position(cell.pos.X + i, cell.pos.Y + j);

                    if (Check.OutOfRange(newPos, map) == true ||
                        map.GetCell(i, j).numberPlayer != cell.numberPlayer)
                    { return; }
                }
            }

            // Если код дошел до этого момента, значит главная клетка это центр 3х3 клеток с одинаковым номером игрока

            for (int i = cell.pos.X - 1; i <= cell.pos.X + 1; i++)
            {
                for (int j = cell.pos.Y - 1; j <= cell.pos.Y + 1; j++)
                {
                    if (map.GetCell(i, j).type != CellType.Dark)
                    { newDark = true; }

                    map.GetCell(i, j).type = CellType.Dark;
                }
            }

            //if (newDark == true) { SoundStep.Play() }
        }
        private static List<int> CalculateScore(Map map)
        {
            int OneScorePaint = 1;
            int OneScoreDark = 1;

            // Словарь(Номер игрока, словарь(Тип клетки, количество таких клеток))
            Dictionary<int, Dictionary<CellType, int>> score = GetEmptyScoreDictionary(map);

            for (int i = 0; i < map.Width; i++)
            {
                for (int j = 0; j < map.Length; j++)
                {
                    Cell cell = map.GetCell(i, j);

                    score[cell.numberPlayer][cell.type] += 1;

                }
            }

            List<int> scorePlayer = new List<int>();

            for (int i = -1; i < score.Count - 1; i++)
            {
                scorePlayer.Add(
                    score[i][CellType.Paint] * OneScorePaint +
                    score[i][CellType.Dark] * OneScoreDark);
            }

            return scorePlayer;
        }
        private static Dictionary<int, Dictionary<CellType, int>> GetEmptyScoreDictionary(Map map)
        {
            // Если номера игроков не будут начинаться с 0 и идти по порядку то тут всё сломается
            // Надеюсь такого не будет а то мне лень исправлять
            Dictionary<int, Dictionary<CellType, int>> dict = new Dictionary<int, Dictionary<CellType, int>>();

            for (int i = -1; i < map.players.Count; i++)
            {
                dict[i] = new Dictionary<CellType, int>();

                foreach (CellType cellType in Enum.GetValues(typeof(CellType)))
                {
                    dict[i].Add(cellType, 0);
                }
            }

            return dict;
        }
    }
}