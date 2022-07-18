using System.Collections.Generic;
namespace ColorChessModel
{
    public class Figure
    {
        public Position? pos;
        public FigureType type;
        public Player? player;
        public Dictionary<CellType, bool>[]? require;


        public Figure() { }

        public Figure(Figure anotherFigure, Player newPlayer)
        {
            this.pos = new Position(anotherFigure.pos);
            this.type = anotherFigure.type;
            this.player = newPlayer;
            this.require = anotherFigure.require;
            //FigureView?
        }

        public int Number
        {
            get { return player.number; }
        }

        //public static bool operator !=(Figure figure1, Figure figure2)
        //{
        //    return !(figure1 == figure2);
        //}

        public bool equals(Figure anotherFigure)
        {
            return this.pos == anotherFigure.pos && this.type == anotherFigure.type && this.Number == anotherFigure.Number;
        }

        //public static bool operator ==(Figure figure1, Figure figure2)
        //{
        //    return figure1.pos == figure2.pos && figure1.type == figure2.type && figure1.Number == figure2.Number;
        //}
    };
}