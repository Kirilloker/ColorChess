using System;
using System.Collections.Generic;
namespace ColorChessModel
{
    class CastleAlgorithm : WayCalcStrategy
    {
        public List<Cell> AllSteps(Map map, Figure figure)
        {
            List<Cell> avaibleCell = new List<Cell>();

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

                    avaibleCell.Add(cell);
                }
            }
            return avaibleCell;
        }

        public List<Cell> Way(Map map, Position startPos, Position endPos, Figure figure)
        {
            // Не уверен насчёт правильности, но вроде выглядит не плохо
            List<Cell> way = new List<Cell>();

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