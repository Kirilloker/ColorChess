using System;
using System.Collections.Generic;
namespace ColorChessModel
{
    class BishopAlgorithm : WayCalcStrategy
    {
        public List<Cell> AllSteps(Map map, Figure figure)
        {
            List<Cell> avaibleCell = new List<Cell>();

            Position posFigure = figure.pos;


            for (int i = -3; i <= 3; i += 2)
            {
                for (int j = 0; j < map.Width; j++)
                {
                    Position posCell = new Position(posFigure.X + j * (i % 2), posFigure.Y + j * (i % 2) * (Math.Abs(i) - 2));

                    if (Check.OutOfRange(posCell, map) == true) { break; }
                    if (Check.SelfPoint(posCell, posFigure) == true) { continue; }

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
            List<Cell> way = new List<Cell>();

            for (int i = -3; i <= 3; i += 2)
            {
                way.Clear();

                for (float j = 0; j < map.Width; j++)
                {
                    Position posCell = new Position((startPos.X + j * (i % 2)), (startPos.Y + j * (i % 2) * (Math.Abs(i) - 2)));

                    if (Check.OutOfRange(posCell, map) == true) { continue; }

                    way.Add(map.GetCell(posCell));

                    if (posCell == endPos) { return way; }
                }
            }



            return new List<Cell>();
        }

    }
}