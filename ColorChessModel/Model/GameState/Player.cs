using System.Collections.Generic;


namespace ColorChessModel
{
    public class Player
    {
        private int number;
        private CornerType corner;
        private ColorType color;
        private PlayerType type;
        private List<Figure> figures = new();

        public Player() { }

        public Player(Player anotherPlayer)
        {
            this.number = anotherPlayer.number;
            this.corner = anotherPlayer.corner;
            this.color = anotherPlayer.color;
            this.type = anotherPlayer.type;

            this.figures = new List<Figure>();

            for (int i = 0; i < anotherPlayer.figures.Count; i++)
                this.figures.Add(new Figure(anotherPlayer.figures[i], this));
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;

            Player other = obj as Player;
            if (other == null) return false;

            if (figures.Count != other.figures.Count) return false;

            for (int i = 0; i < figures.Count; i++)
                if (figures[i].Equals(other.figures[i]) == false) return false;


            return number == other.number &&
                   color == other.color &&
                   corner == other.corner &&
                   type == other.type;
        }

        public int Number { get => number; set => number = value; }
        public CornerType Corner { get => corner; set => corner = value; }
        public ColorType Color { get => color; set => color = value; }
        public PlayerType Type { get => type; set => type = value; }
        public List<Figure> Figures { get => figures; set => figures = value; }

    };
}