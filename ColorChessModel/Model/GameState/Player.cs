using System.Collections.Generic;


namespace ColorChessModel
{
    public class Player
    {
        private int number;
        private CornerType corner;
        private ColorType color;
        private PlayerType type;
        private List<Figure> figures = new List<Figure>();

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

        public int Number { get => number; set => number = value; }
        public CornerType Corner { get => corner; set => corner = value; }
        public ColorType Color { get => color; set => color = value; }
        public PlayerType Type { get => type; set => type = value; }
        public List<Figure> Figures { get => figures; set => figures = value; }

    };
}