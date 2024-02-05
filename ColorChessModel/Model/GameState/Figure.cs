

namespace ColorChessModel
{
    public class Figure
    {
        private Position pos;
        private FigureType type;
        private Player player;
        private Dictionary<CellType, bool>[] require;

        public Figure() { }

        public Figure(Figure anotherFigure, Player newPlayer)
        {
            this.pos = new Position(anotherFigure.pos);
            this.type = anotherFigure.type;
            this.player = newPlayer;
            this.require = anotherFigure.require;
        }

        public bool Equals(Figure anotherFigure)
        {
            return  this.Pos == anotherFigure.Pos && 
                    this.Type == anotherFigure.Type && 
                    this.Number == anotherFigure.Number;
        }

        public int Number { get => Player.Number; }
        public Position Pos { get => pos; set => pos = value; }
        public FigureType Type { get => type; set => type = value; }
        public Player Player { get => player; set => player = value; }
        public Dictionary<CellType, bool>[] Require { get => require; set => require = value; }
    };
}