
using System;
using System.Collections;
using System.Security.Cryptography.X509Certificates;

namespace Grid.Domain
{
    public class World
    {

        public int Height
        {
            get;
        }
        public int Width
        {
            get;
        }

        public Location Start { get; }

        public Location Goal { get; }

        private BitArray _isBlockedLocations;

        public World(string gridString)
        {
            var lines =  gridString.Split( new[] { Environment.NewLine },StringSplitOptions.None);
            if (lines.Length < 3)
            {
                throw new GridTooSmallException();
            }
            Height = lines.Length - 1; //Last line has Enter - allways
            Width = lines[0].Length;
            _isBlockedLocations = new BitArray(Width*Height);
            for (int i = 0; i < lines.Length; i++)
            {
                if (i != lines.Length-1 && lines[i].Length != Width)
                {
                    throw new GridWithDifferentLinesSize();
                }
                for (int j = 0; j < lines[i].Length; j++)
                {
                    if (lines[i][j] == '@')
                    {
                        Start = new Location(i, j);
                    }
                    else if (lines[i][j] == '*')
                    {
                        Goal = new Location(i, j);
                    }
                    else if (lines[i][j] == '#')
                    {
                        _isBlockedLocations[i * Width + j] = true;
                    }
                }
            }
            if (Start == null)
            {
                throw new GridStartNotFoundException();
            }
            if (Goal == null)
            {
                throw new GridGoalNotFoundException();
            }

        }

        public bool isBlocked(Location loc)
        {
            return _isBlockedLocations[loc.y * Width + loc.x];
        }

//        void readGrid(string filename)
//        {
//            string line;
//            ifstream myfile(filename);
//            if (myfile.is_open())
//            {
//                while (getline(myfile, line))
//                {
//                    grid.push_back(line);
//                    for (unsigned int i = 0; i < line.length(); i++)
//                    {
//                        if (line[i] == '@')
//                        {
//                            agent.y = grid.size() - 1;
//                            agent.x = i;
//                        }
//                        else if (line[i] == '*')
//                        {
//                            goal.y = grid.size() - 1;
//                            goal.x = i;
//                        }
//                    }
//                }
//                myfile.close();
//            }
//            height = grid.size();
//            width = grid[0].length();
//
//            for (unsigned int i = 0; i < height; i++)
//            {
//                assert(width == grid[i].length());
//            }
//            binaryGridSize = ceil((double)(width * height) / 8);
//        }
    }

    public class GridStartNotFoundException : Exception
    {
    }

    public class GridGoalNotFoundException : Exception
    {
    }

    public class GridTooSmallException : Exception
    {
    }
    public class GridWithDifferentLinesSize : Exception
    {
    }
}
