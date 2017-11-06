
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
            return $"({X},{Y})";
        }

    }
}
