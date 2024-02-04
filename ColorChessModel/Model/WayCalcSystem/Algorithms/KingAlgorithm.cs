using System;
using System.Collections.Generic;
using System.Linq;

namespace ColorChessModel
{
    class KingAlgorithm : IWayCalcStrategy
    {
        public List<Cell> AllSteps(Map map, Figure figure)
        {
            Dictionary<Cell, int> dict = new Dictionary<Cell, int>(100);

           // List<Cell> avaibleCell = new List<Cell>(100);
            Position posFigure = figure.pos;

            for (float i = 0; i < map.Width; i++)
            {
                for (float j = 0; j < map.Length; j++)
                {
                    Position posCell = new Position(i, j);

                    Cell cell = map.GetCell(posCell);

                    if (Check.SelfPoint(posCell, posFigure) == true ||
                        Check.BusyCell(cell) == true ||
                       (Check.Avaible(posCell, figure, map) == false))
                    { continue; }


                    int test = 0;
                    test += (Check.BusyCell(cell)) ? 1 : 0;
                    test += (Check.SelfCellDark(cell, figure.Number)) ? -3 : 0;

                    dict.Add(cell, test);
                    //avaibleCell.Add(cell);
                }
            }

            List<Cell> avaibleCell = new List<Cell>(dict.Count);

            // Сортируем словарь и добовляем всё в массив
            dict = dict.OrderBy(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);

            foreach (Cell cell in dict.Keys)
            {
                avaibleCell.Add(cell);
            }

            avaibleCell.Reverse();


            return avaibleCell;
        }

        public List<Cell> Way(Map map, Position startPos, Position endPos, Figure figure)
        {
            List<Cell> way = new List<Cell>(2);

            way.Add(map.GetCell(startPos));
            way.Add(map.GetCell(endPos));

            return way;
        }
    }
}