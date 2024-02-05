

namespace ColorChessModel
{
    public class Cell
    {
        private Position pos;
        private CellType type;
        private Figure figure;
        private int numberPLayer;

        public Cell() { }

        public Cell(Cell anotherCell)
        {
            this.pos = new Position(anotherCell.pos);
            this.type = anotherCell.type;
            this.figure = null;
            this.numberPLayer = anotherCell.numberPLayer;
        }


        public bool Available(Dictionary<CellType, bool>[] require, int numberPlayerFigure)
        {
            // Может ли фигура наступить на такой тип клетки
            if (numberPlayerFigure == numberPLayer)
            {
                if (require[0].ContainsKey(Type) == true) return require[0][Type];
                else return false;
            }
            else
            {
                if (require[1].ContainsKey(Type) == true) return require[1][Type]; 
                else return false; 
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true; 

            Cell other = obj as Cell;
            if (other == null) return false;
            
            if (figure != null && (figure.Equals(other.figure) == false)) return false;

            return pos == other.pos &&
                   type == other.type &&
                   numberPLayer == other.numberPLayer;
        }



        public FigureType FigureType { get => Figure != null ? Figure.Type : FigureType.Empty; }

        public Position Pos { get => pos; set => pos = value; }
        public CellType Type { get => type; set => type = value; }
        public Figure Figure { get => figure; set => figure = value; }
        public int NumberPlayer { get => numberPLayer; set => numberPLayer = value; }
    }
}