using System.Collections.Generic;
using System.Linq;

namespace ColorChessModel
{
    class PawnAlgorithm : IWayCalcStrategy
    {
        public List<Cell> AllSteps(Map map, Figure figure)
        {
            //List<Cell> avaibleCell = new List<Cell>(8);
            Dictionary<Cell, int> dict = new Dictionary<Cell, int>(10);

            Position posFigure = figure.pos;

            for (int i = posFigure.X - 1; i <= posFigure.X + 1; i++)
            {
                for (int j = posFigure.Y - 1; j <= posFigure.Y + 1; j++)
                {
                    Position posCell = new Position(i, j);

                    if ((Check.OutOfRange(posCell, map) == true ||
                        Check.SelfPoint(posCell, posFigure)) == true) { continue; }

                    Cell cell = map.GetCell(posCell);


                    // Чтобы не съесть свою фигуру
                    if (cell.numberPlayer == figure.Number &&
                        cell.FigureType != FigureType.Empty) { continue; }

                    if (Check.Avaible(posCell, figure, map) == false) { continue; }

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

            //DebugConsole.Print("PAWN");
            foreach (Cell cell in dict.Keys)
            {
                //DebugConsole.Print(dict[cell].ToString());
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