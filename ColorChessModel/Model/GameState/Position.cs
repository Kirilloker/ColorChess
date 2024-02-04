using System.Collections.Generic;
namespace ColorChessModel
{
    public class Position
    {
        private int x;
        private int y;

        public Position(int _x, int _y)
        {
            if (_x < 0 || _y < 0)
                throw new ArgumentException("Coordinates cannot be negative.");

            x = _x;
            y = _y;
        }

        public Position(Position anotherPosition)
        {
            this.x = anotherPosition.x;
            this.y = anotherPosition.y;
        }

        public static bool operator !=(Position pos1, Position pos2)
        {
            return !(pos1 == pos2);
        }

        public static bool operator ==(Position pos1, Position pos2)
        {
            return pos1.x == pos2.x && pos1.y == pos2.y;
        }

        public override string ToString()
        {
            return "(X:" + x + ";   Y:" + y + ")";
        }

        public int X 
        { 
            get { return x; } 
            set { x = value; } 
        }

        public int Y
        {
            get { return y; }
            set { y = value; }
        }
    }
}