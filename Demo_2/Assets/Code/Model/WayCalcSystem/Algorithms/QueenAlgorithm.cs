using System;
using System.Collections.Generic;
namespace ColorChessModel
{
    class QueenAlgorithm : WayCalcStrategy
    {
        public List<Cell> AllSteps(Map map, Figure figure)
        {
            List<Cell> avaibleCell = new List<Cell>();

            Position posFigure = figure.pos;

            for (int i = -3; i <= 3; i += 2)
            {
                if (Math.Abs(i) == 3)
                {
                    for (int j = 0; j < map.Width; j++)
                    {
                        Position posCell = new Position(posFigure.X + j * (i % 2), posFigure.Y + j * (i % 2));

                        if (Check.OutOfRange(posCell, map) == true) { break; }

                        if (Check.SelfPoint(posCell, posFigure) == true) { continue; }

                        Cell cell = map.GetCell(posCell);

                        if (Check.BusyCell(cell) == true
                            ||
                            Check.Avaible(posCell, figure, map) == false) { break; }

                        avaibleCell.Add(cell);
                    }

                    for (int j = 0; j < map.Width; j++)
                    {
                        Position posCell = new Position(posFigure.X + j * (i % 2), posFigure.Y);

                        if (Check.OutOfRange(posCell, map) == true) { break; }

                        if (Check.SelfPoint(posCell, posFigure) == true) { continue; }

                        Cell cell = map.GetCell(posCell);

                        if (Check.BusyCell(cell) == true
                            ||
                            Check.Avaible(posCell, figure, map) == false) { break; }

                        avaibleCell.Add(cell);
                    }
                }
                else
                {
                    for (int j = 0; j < map.Length; j++)
                    {
                        Position posCell = new Position(posFigure.X + j * (i % 2), posFigure.Y - j * (i % 2));

                        if (Check.OutOfRange(posCell, map) == true) { break; }

                        if (Check.SelfPoint(posCell, posFigure) == true) { continue; }

                        Cell cell = map.GetCell(posCell);

                        if (Check.BusyCell(cell) == true
                            ||
                            Check.Avaible(posCell, figure, map) == false) { break; }

                        avaibleCell.Add(cell);
                    }

                    for (int j = 0; j < map.Length; j++)
                    {
                        Position posCell = new Position(posFigure.X, posFigure.Y + j * (i % 2));

                        if (Check.OutOfRange(posCell, map) == true) { break; }

                        if (Check.SelfPoint(posCell, posFigure) == true) { continue; }

                        Cell cell = map.GetCell(posCell);

                        if (Check.BusyCell(cell) == true
                            ||
                            Check.Avaible(posCell, figure, map) == false) { break; }

                        avaibleCell.Add(cell);
                    }
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

                if (Math.Abs(i) == 3)
                {
                    for (int j = 0; j < map.Width; j++)
                    {
                        Position posCell = new Position(startPos.X + j * (i % 2), startPos.Y);

                        if (Check.OutOfRange(posCell, map) == true) { break; }

                        way.Add(map.GetCell(posCell));

                        if (posCell == endPos) { return way; }
                    }

                    way.Clear();

                    for (int j = 0; j < map.Width; j++)
                    {
                        Position posCell = new Position(startPos.X + j * (i % 2), startPos.Y + j * (i % 2));

                        if (Check.OutOfRange(posCell, map) == true) { break; }

                        way.Add(map.GetCell(posCell));

                        if (posCell == endPos) { return way; }
                    }
                }
                else
                {
                    for (int j = 0; j < map.Length; j++)
                    {
                        Position posCell = new Position(startPos.X, startPos.Y + j * (i % 2));

                        if (Check.OutOfRange(posCell, map) == true) { break; }

                        way.Add(map.GetCell(posCell));

                        if (posCell == endPos) { return way; }
                    }

                    way.Clear();

                    for (int j = 0; j < map.Length; j++)
                    {
                        Position posCell = new Position(startPos.X + j * (i % 2), startPos.Y - j * (i % 2));

                        if (Check.OutOfRange(posCell, map) == true) { break; }

                        way.Add(map.GetCell(posCell));

                        if (posCell == endPos) { return way; }
                    }
                }
            }

            return new List<Cell>();
        }
    }
}