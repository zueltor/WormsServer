using System;
using static System.Int32;

namespace WormsServer
{
    public struct Position
    {
        public int X { get; set; }
        public int Y { get; set; }

        private static readonly Position invalidPosition = new(MaxValue, MaxValue);

        public Position(int x = 0, int y = 0)
        {
            X = x;
            Y = y;
        }

        public static Position InvalidPosition()
        {
            return invalidPosition;
        }

        public bool IsNothing()
        {
            return X == 0 && Y == 0;
        }

        public int Distance(Position pos)
        {
            return Math.Abs(pos.X - X) + Math.Abs(pos.Y - Y);
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }

        public override bool Equals(object obj)
        {
            if (obj is not Position position)
            {
                return false;
            }

            return position.X == X && position.Y == Y;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        public static bool operator ==(Position left, Position right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Position left, Position right)
        {
            return !(left == right);
        }
    }
}