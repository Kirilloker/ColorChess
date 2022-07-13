using System.Collections.Generic;
namespace ColorChessModel
{
    public class Cell
    {
        public Position pos;
        public CellType type;
        public Figure? figure;
        public int numberPlayer;
        public Cell() { }

#if DEBUG
        public Cell(Position pos)
        {
            this.pos = pos;
            this.type = CellType.Empty;
            this.figure = null;
            numberPlayer = -1;
        }
#endif

        public Cell(Cell anotherCell)
        {
            this.pos = new Position(anotherCell.pos);
            this.type = anotherCell.type;
            this.figure = null;
            this.numberPlayer = anotherCell.numberPlayer;
        }


        public FigureType FigureType
        {
            get
            {
                if (this.figure == null) return FigureType.Empty;
                return figure.type;
            }
        }




        public bool Avaible(Dictionary<CellType, bool>[] require, int numberPlayerFigure)
        {
            // Может ли фигура наступить на такой тип клетки

            if (numberPlayerFigure == numberPlayer)
            {
                if (require[0].ContainsKey(type) == true) { return require[0][type]; }
                else { return false; }
            }
            else
            {
                if (require[1].ContainsKey(type) == true) { return require[1][type]; }
                else { return false; }
            }
        }
    }
}