using System.Collections.Generic;
namespace ColorChessModel
{
    public class Player
    {
        public int number;
        public CornerType corner;
        public ColorType color;
        public PlayerType type;

        public List<Figure> figures = new List<Figure>();


        public Player() { }

        public Player(Player anotherPlayer)
        {
            this.number = anotherPlayer.number;
            this.corner = anotherPlayer.corner;
            this.color = anotherPlayer.color;
            this.type = anotherPlayer.type;

            this.figures = new List<Figure>();

            for (int i = 0; i < anotherPlayer.figures.Count; i++)
            {
                this.figures.Add(new Figure(anotherPlayer.figures[i], this));
            }
        }

        public string GetStringForHash()
        {
            string stringForHash = number.ToString();

            for (int i = 0; i < this.figures.Count; i++)
            {
                stringForHash += figures[i].GetStringForHash();
            }

            return stringForHash; 
        }
    };
}