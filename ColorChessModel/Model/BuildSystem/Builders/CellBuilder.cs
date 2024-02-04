using System.Collections.Generic;
namespace ColorChessModel
{
    class CellBuilder
    {
        private Cell cell;
        public CellBuilder()
        {
            cell = new Cell();
        }

        public Cell MakeCell(Position pos, CellType cellType)
        {
            cell.Pos = pos;
            cell.Type = cellType;
            cell.Figure = null;
            cell.NumberPlayer = -1;
            return new Cell(cell);
        }
    }
}