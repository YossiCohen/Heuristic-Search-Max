
using System;

namespace Grid.Domain
{
    public class Location
    {

        public int X
        {
            get;
        }
        public int Y
        {
            get;
        }

        public Location()
        {
            X = 0;
            Y = 0;
        }

        public Location(int x, int y)
        {
            X = x;
            Y = y;
        }
        public Location(Location other)
        {
            X = other.X;
            Y = other.Y;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Location other))
            {
                return false;
            }
            return X == other.X && Y == other.Y;
        }

        protected bool Equals(Location other)
        {
            return X == other.X && Y == other.Y;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }

        public override string ToString()
        {
            return $"({X}_{Y})";
        }

        public Location GetMovedLocation(MoveDirection direction)
        {
            Location newLocation; 
            switch (direction)
            {
                case MoveDirection.Up:
                    newLocation = new Location(X, Y - 1);
                    break;
                case MoveDirection.Down:
                    newLocation = new Location(X, Y + 1);
                    break;
                case MoveDirection.Left:
                    newLocation = new Location(X - 1, Y);
                    break;
                case MoveDirection.Right:
                    newLocation = new Location(X + 1, Y);
                    break;
                case MoveDirection.Wait:
                    newLocation = new Location(this);
                    break;
                default:
                     throw new NotImplementedException();
            }
            return newLocation;
        }

        public int GetLinearLocationRepresentation(World w)
        {
            return Y * w.Width + X;
        }
    }

    public enum MoveDirection { Up, Down, Left, Right, Wait}
}
