using System.Collections.Generic;
namespace ColorChessModel
{
    public class Position
    {
        public int X;
        public int Y;

        public Position(int _X, int _Y)
        {
            X = _X;
            Y = _Y;
        }

        public Position(float _X, float _Y)
        {
            X = (int)_X;
            Y = (int)_Y;
        }

        public Position(Position anotherPosition)
        {
            this.X = anotherPosition.X;
            this.Y = anotherPosition.Y;
        }

        public static bool operator !=(Position pos1, Position pos2)
        {
            return !(pos1 == pos2);
        }

        public static bool operator ==(Position pos1, Position pos2)
        {
            return pos1.X == pos2.X && pos1.Y == pos2.Y;
        }

        public override string ToString()
        {
            return "(X:" + X + ";   Y:" + Y + ")";
        }
    };
}