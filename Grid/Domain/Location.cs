
using System;

namespace Grid.Domain
{
    public class Location
    {

        public int x
        {
            get;
        }
        public int y
        {
            get;
        }

        public Location()
        {
            this.x = 0;
            this.y = 0;
        }

        public Location(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public Location(Location other)
        {
            this.x = other.x;
            this.y = other.y;
        }

        public override bool Equals(object obj)
        {
            var other = obj as Location;

            if (other == null)
            {
                return false;
            }

            return (this.x == other.x) && (this.y == other.y);
        }

        public override string ToString()
        {
            return $"({this.x},{this.y})";
        }
    }
}
