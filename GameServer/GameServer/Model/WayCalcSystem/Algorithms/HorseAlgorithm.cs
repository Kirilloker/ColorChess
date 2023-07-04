using System;
using System.Collections.Generic;
using System.Linq;

namespace ColorChessModel
{
    class HorseAlgorithm : IWayCalcStrategy
    {
        public List<Cell> AllSteps(Map map, Figure figure)
        {
            //List<Cell> avaibleCell = new List<Cell>(8);
            Dictionary<Cell, int> dict = new Dictionary<Cell, int>(8);

            Position posFigure = figure.pos;

            for (int i = (posFigure.X - 2); i <= posFigure.X + 2; i++)
            {
                if (i < 0 || i > map.Width - 1) { continue; }

                for (int j = (posFigure.Y - 2); j <= posFigure.Y + 2; j++)
                {
                    if (j < 0 || j > map.Length - 1) { continue; }

                    if (((Math.Abs(i - posFigure.X) == 1) && (Math.Abs(j - posFigure.Y) == 2)) ||
                        ((Math.Abs(i - posFigure.X) == 2) && (Math.Abs(j - posFigure.Y) == 1)))
                    {
                        Position posCell = new Position(i, j);

                        Cell cell = map.GetCell(posCell);

                        if (Check.BusyCell(cell) == true
                            ||
                            Check.Avaible(posCell, figure, map) == false) { continue; }

                        int test = 0;
                        //test += (Check.BusyCell(cell)) ? 1 : 0;
                        test += (Check.SelfCellDark(cell, figure.Number)) ? -3 : 0;

                        dict.Add(cell, test);
                    }
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
            List<Cell> way = new List<Cell>(4);

            way.Add(map.GetCell(startPos));

            Position pos1 = new Position(startPos.X, startPos.Y + (endPos.Y - startPos.Y) / 2);
            Position pos2 = new Position(startPos.X, startPos.Y + (endPos.Y - startPos.Y));

            if ((Math.Abs(endPos.X - startPos.X)) > (Math.Abs(endPos.Y - startPos.Y)))
            {
                pos1 = new Position(startPos.X + (endPos.X - startPos.X) / 2, startPos.Y);
                pos2 = new Position(startPos.X + (endPos.X - startPos.X), startPos.Y);
            }


            if (jump_horse(pos1, map, figure))
            {
                way.Add(map.GetCell(pos1));
            }

            if (jump_horse(pos2, map, figure))
            {
                way.Add(map.GetCell(pos2));
            }


            way.Add(map.GetCell(endPos));

            return way;
        }


        bool jump_horse(Position posCell, Map map, Figure figure)
        {
            Cell cell = map.GetCell(posCell);

            if (Check.BusyCell(cell) == true
                ||
                Check.Avaible(posCell, figure, map) == false) { return false; }

            return true;
        }
    }
}