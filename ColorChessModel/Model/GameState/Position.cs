using System.Collections.Generic;
namespace ColorChessModel
{
    public class Position
    {
        public int X;
        public int Y;

        public Position()
        {
            X = -10;
            Y = -10;
        }

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

        public override int GetHashCode()
        {
            return this.X.GetHashCode() * this.Y.GetHashCode();
        }

        public int _X 
        { 
            get { return X; } 
            set { X = value; } 
        }

        public int _Y
        {
            get { return Y; }
            set { Y = value; }
        }

        public string GetStringForHash()
        {
            return X.ToString() + Y.ToString();
        }

        public override bool Equals(object obj)
        {
            return obj is Position position &&
                   X == position.X &&
                   Y == position.Y &&
                   _X == position._X &&
                   _Y == position._Y;
        }
    }
}