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
            cell.pos = pos;
            cell.type = cellType;
            cell.figure = null;
            cell.numberPlayer = -1;
            return new Cell(cell);
        }
    }
}