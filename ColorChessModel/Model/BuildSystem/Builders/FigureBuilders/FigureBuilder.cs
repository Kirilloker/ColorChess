using System.Collections.Generic;
namespace ColorChessModel
{
    public class FigureBuilder
    {
        protected Figure figure;

        public FigureBuilder()
        {
            figure = new Figure();
        }

        public Figure GetResult()
        {
            return figure;
        }

        public void SetPosition(Position _position)
        {
            figure.Pos = _position;
        }


        public void SetPlayer(Player _player)
        {
            figure.Player = _player;
        }

        public virtual void SetFigRequire()
        {

        }

        public virtual void SetFigType()
        {

        }

    }
}