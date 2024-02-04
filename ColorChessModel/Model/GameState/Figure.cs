using System.Collections.Generic;
namespace ColorChessModel
{
    public class Figure
    {
        public Position pos;
        public FigureType type;
        public Player player;
        public Dictionary<CellType, bool>[] require;


        public Figure() { }

        public Figure(Figure anotherFigure, Player newPlayer)
        {
            this.pos = new Position(anotherFigure.pos);
            this.type = anotherFigure.type;
            this.player = newPlayer;
            this.require = anotherFigure.require;
        }

        public int Number
        {
            get { return player.number; }
        }

        public bool Equals(Figure anotherFigure)
        {
            return this.pos == anotherFigure.pos && this.type == anotherFigure.type && this.Number == anotherFigure.Number;
        }

        public override int GetHashCode()
        {
            return
                this.pos.GetHashCode() *
                this.type.GetHashCode();
        }


        public string GetStringForHash()
        {
            return pos.GetStringForHash() + Number.ToString();
        }
    };
}