using System.Collections.Generic;
namespace ColorChessModel
{
    public static class Check
    {
        public static bool OutOfRange(Position posCell, Map map)
        {
            if (posCell.X < 0 ||
                posCell.Y < 0 ||
                posCell.X > map.Width - 1 ||
                posCell.Y > map.Length - 1)
            { return true; }

            return false;
        }

        public static bool SelfPoint(Position posCell, Position posFigure)
        {
            if (posCell == posFigure) { return true; }

            return false;
        }

        public static bool SelfCellDark(Cell cell, int numberPlayer)
        {
            if ((cell.numberPlayer == numberPlayer) && (cell.type == CellType.Dark)) { return true; }
            return false;
        }

        public static bool Avaible(Position posCell, Figure figure, Map map)
        {
            return map.GetCell(posCell).Avaible(figure.require, figure.Number);
        }


        public static bool BusyCell(Cell cell)
        {
            if (cell.FigureType != FigureType.Empty) { return true; }
            return false;
        }

        public static bool BesideEnemy(Position posCell, Map map, int numberPlayer)
        {
            if (map.GetCell(posCell).type == CellType.Dark) return false;

            for (int i = posCell.X - 1; i <= posCell.X + 1; i++)
            {
                for (int j = posCell.Y - 1; j <= posCell.Y + 1; j++)
                {
                    Position posCellCheck = new Position(i, j);
                    if (OutOfRange(posCellCheck, map) == true) continue;

                    Cell cell = map.GetCell(posCellCheck);

                    if ((cell.figure != null) && (cell.numberPlayer == numberPlayer))
                        return true;
                }
            }


            return false;
        }
    }
}