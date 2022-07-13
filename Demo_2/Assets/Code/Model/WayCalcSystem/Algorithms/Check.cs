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

        public static bool Avaible(Position posCell, Figure figure, Map map)
        {
            return map.GetCell(posCell).Avaible(figure.require, figure.Number);
        }


        public static bool BusyCell(Cell cell)
        {
            if (cell.FigureType != FigureType.Empty) { return true; }
            return false;
        }
    }
}