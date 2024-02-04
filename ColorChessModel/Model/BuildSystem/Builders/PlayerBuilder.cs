using System.Collections.Generic;
namespace ColorChessModel
{
    class PlayerBuilder
    {
        private Player player;

        public PlayerBuilder()
        {
            player = new Player();
        }

        public Player MakePlayer(CornerType corner, ColorType color, PlayerType type, int number)
        {
            player.Number = number;
            player.Corner = corner;
            player.Color = color;
            player.Type = type;

            return new Player(player);
        }
    }
}