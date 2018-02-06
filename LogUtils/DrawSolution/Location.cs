using System;

namespace DrawSolution
{
    class Location
    {
        public enum Direction
        {
            NA,Up, Down, Left, Right
        }

        private int x;
        private int y;

        public Location()
        {
            this.X = 0;
            this.Y = 0;
        }

        public Location(Location dup)
        {
            this.X = dup.X;
            this.Y = dup.Y;
        }

        public Location(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public Location(string loc)
        {
            var spl = loc.Split(',');
            this.X = Convert.ToInt32(spl[0]);
            this.Y = Convert.ToInt32(spl[1]);
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

        public Direction getDirectionTo(Location next)
        {
            if (this.X == next.X)
            {
                if (this.Y > next.Y)
                {
                    return Direction.Up;
                }
                else if (this.Y < next.Y)
                {
                    return Direction.Down;
                }
            }
            else if (this.Y == next.Y)
            {
                if (this.X > next.X)
                {
                    return Direction.Left;
                }
                else if (this.X < next.X)
                {
                    return Direction.Right;
                }
            }
            return Direction.NA;
        }

        public string ToString()
        {
            return ""+X + ',' + Y;
        }
    }
}