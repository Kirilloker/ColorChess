using System.Collections.Generic;
namespace ColorChessModel
{
    class KingAlgorithm : WayCalcStrategy
    {
        public List<Cell> AllSteps(Map map, Figure figure)
        {
            List<Cell> avaibleCell = new List<Cell>();
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


                    avaibleCell.Add(cell);
                }
            }

            return avaibleCell;
        }

        public List<Cell> Way(Map map, Position startPos, Position endPos, Figure figure)
        {
            List<Cell> way = new List<Cell>();

            way.Add(map.GetCell(startPos));
            way.Add(map.GetCell(endPos));

            return way;
        }
    }
}