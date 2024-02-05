using System;
using System.Collections.Generic;


namespace ColorChessModel
{
    public class GameStateCalcSystem
    {
        public static Map UpdateGameState(Map gameState)
        {
            CheckCapture(gameState);
            gameState.Score = CalculateScore(gameState);

            return gameState;
        }

        public static Map ApplyStep(Map _gameState, Figure figure, Cell endCell)
        {
            Map gameState = new Map(_gameState);

            // Получаем ссылку на новую фигуру, которая делает ход (потому что создалась копия карты)
            Figure newFigure = gameState.GetCell(figure.Pos).Figure;
            
            // Если в клетке в которую хотят сходить стоит фигура -> её хотят съесть
            if (endCell.Figure != null)
                gameState.KillFigure(endCell.Figure);

            // Тут ошибка бывает
            List<Cell> Way = WayCalcSystem.CalcWay(gameState, newFigure.Pos, endCell.Pos, newFigure);

            for (int i = 0; i < Way.Count; i++)
            {
                Way[i].NumberPlayer = newFigure.Number;
                Way[i].Type = CellType.Paint;
            }

            Way[0].Figure = null;
            Way[Way.Count - 1].Figure = newFigure;
            newFigure.Pos = new Position(endCell.Pos);


            UpdateGameState(gameState);

            gameState.CountStep++;

            return gameState;
        }

        public static Map UpdateGameStateForBuilder(Map gameState)
        {
            foreach (Player player in gameState.Players)
            {
                foreach (Figure figure in player.Figures)
                {
                    Cell cell = gameState.GetCell(figure.Pos);

                    cell.Figure = figure;
                    cell.NumberPlayer = figure.Number;

                    cell.Type = CellType.Paint;
                }
            }

            gameState = UpdateGameState(gameState);

            return gameState;
        }


        private static void CheckCapture(Map map)
        {
            // Делает квадраты захваченными

            for (int i = 0; i < map.Width; i++)
            {
                for (int j = 0; j < map.Length; j++)
                {

                    if ((map.GetCell(i, j).Type != CellType.Empty) &&
                        (map.GetCell(i, j).NumberPlayer != -1))
                    { MakeCapture(map, map.GetCell(i, j)); }
                }
            }
        }
        private static void MakeCapture(Map map, Cell cell)
        {
            for (int i = cell.Pos.X - 1; i <= cell.Pos.X + 1; i++)
            {
                for (int j = cell.Pos.Y - 1; j <= cell.Pos.Y + 1; j++)
                {
                    Position newPos = new Position(i, j);

                    if (Check.OutOfRange(newPos, map) == true ||
                        map.GetCell(i, j).NumberPlayer != cell.NumberPlayer)
                    return;
                }
            }

            // Если код дошел до этого момента, значит главная клетка это центр 3х3 клеток с одинаковым номером игрока

            for (int i = cell.Pos.X - 1; i <= cell.Pos.X + 1; i++)
                for (int j = cell.Pos.Y - 1; j <= cell.Pos.Y + 1; j++)
                    map.GetCell(i, j).Type = CellType.Dark;
        }
        private static Dictionary<int, Dictionary<CellType, int>> CalculateScore(Map map)
        {
            // Словарь(Номер игрока, словарь(Тип клетки, количество таких клеток))
            Dictionary<int, Dictionary<CellType, int>> score = GetEmptyScoreDictionary(map);

            for (int i = 0; i < map.Width; i++)
            {
                for (int j = 0; j < map.Length; j++)
                {
                    Cell cell = map.GetCell(i, j);

                    score[cell.NumberPlayer][cell.Type] += 1;
                }
            }

            return score;

        }
        private static Dictionary<int, Dictionary<CellType, int>> GetEmptyScoreDictionary(Map map)
        {
            // Если номера игроков не будут начинаться с 0 и идти по порядку то тут всё сломается
            // Надеюсь такого не будет а то мне лень исправлять
            Dictionary<int, Dictionary<CellType, int>> dict = new Dictionary<int, Dictionary<CellType, int>>();

            for (int i = -1; i < map.PlayersCount; i++)
            {
                dict[i] = new Dictionary<CellType, int>();

                foreach (CellType cellType in Enum.GetValues(typeof(CellType)))
                    dict[i].Add(cellType, 0);
            }

            return dict;
        }
    }
}