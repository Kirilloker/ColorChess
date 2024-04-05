

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
            if (ReferenceEquals(other, null)) return false;
            
            if (figure != null && (figure.Equals(other.figure) == false)) return false;

            return pos == other.pos &&
                   type == other.type &&
                   numberPLayer == other.numberPLayer;
        }

        public static bool operator !=(Cell cell1, Cell cell2)
        {
            return !(cell1 == cell2);
        }

        public static bool operator ==(Cell cell1, Cell cell2)
        {
            if (ReferenceEquals(cell1, cell2)) return true;
            if (ReferenceEquals(cell1, null)) return false;
            if (ReferenceEquals(cell2, null)) return false;

            return cell1.Equals(cell2); 
        }

        public FigureType FigureType { get => Figure != null ? Figure.Type : FigureType.Empty; }

        public Position Pos { get => pos; set => pos = value; }
        public CellType Type { get => type; set => type = value; }
        public Figure Figure { get => figure; set => figure = value; }
        public int NumberPlayer { get => numberPLayer; set => numberPLayer = value; }

        public override int GetHashCode()
        {
            unchecked 
            {
                int hash = 17;
                hash = hash * 23 + pos.GetHashCode();
                hash = hash * 23 + type.GetHashCode();
                hash = hash * 23 + (figure?.GetHashCode() ?? 0);
                hash = hash * 23 + numberPLayer.GetHashCode();
                return hash;
            }
        }
    }
}