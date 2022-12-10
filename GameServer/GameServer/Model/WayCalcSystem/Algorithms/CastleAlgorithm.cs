using System;
using System.Collections.Generic;
using System.Linq;

namespace ColorChessModel
{
    class CastleAlgorithm : IWayCalcStrategy
    {
        public List<Cell> AllSteps(Map map, Figure figure)
        {
            Dictionary<Cell, int> dict = new Dictionary<Cell, int>(20);

            Position posFigure = figure.pos;

            for (int i = -3; i <= 3; i += 2)
            {
                for (int j = 0; j < map.Width; j++)
                {
                    Position posCell = new Position(posFigure.X + j * (i % 2), posFigure.Y);

                    if (Math.Abs(i) == 1)
                    {
                        posCell = new Position(posFigure.X, posFigure.Y + j * (i % 2));
                    }

                    if (Check.OutOfRange(posCell, map)) { break; }

                    if (Check.SelfPoint(posCell, posFigure)) { continue; }

                    Cell cell = map.GetCell(posCell);

                    if (Check.BusyCell(cell) == true
                        ||
                        Check.Avaible(posCell, figure, map) == false) { break; }

                    // Добавляем клетку и расстояние от фигуры до клетки 
                    int test = 0;
                    test += Math.Abs(figure.pos.X - cell.pos.X) + Math.Abs(figure.pos.Y - cell.pos.Y);
                    test += (Check.SelfCellDark(cell, figure.Number)) ? -3 : 0;

                    dict.Add(cell, test);
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
            // Не уверен насчёт правильности, но вроде выглядит не плохо
            List<Cell> way = new List<Cell>(10);

            if (startPos.X == endPos.X)
            {
                for (int i = Math.Min(startPos.Y, endPos.Y); i <= Math.Max(startPos.Y, endPos.Y); i++)
                {
                    way.Add(map.GetCell(new Position(startPos.X, i)));
                }
            }
            else
            {
                for (int i = Math.Min(startPos.X, endPos.X); i <= Math.Max(startPos.X, endPos.X); i++)
                {
                    way.Add(map.GetCell(new Position(i, startPos.Y)));
                }
            }

            if ((endPos.Y < startPos.Y) || (endPos.X < startPos.X)) { way.Reverse(); }

            return way;
        }
    }


}