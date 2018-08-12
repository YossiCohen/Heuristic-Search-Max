
using System;
using System.Collections;

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

        public int LinearSize
        {
            get { return Width * Height; }
        }

        internal readonly BitArray _isBlockedLocations;
        private IGridHeuristic HeuristicFunction { get; }

        public World(string gridString, IGridHeuristic heuristicFunction)
        {
            HeuristicFunction = heuristicFunction;
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
                        Start = new Location(j, i);
                    }
                    else if (lines[i][j] == '*')
                    {
                        Goal = new Location(j, i);
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

        public void InitBcc()
        {
            var bcc = new BiconnectedComponents(this);
            var valid = bcc.GetValidPlacesForMaxPath(Start, Goal);
            for (int i = 0; i < valid.Length; i++)
            {
                if (!valid[i]) _isBlockedLocations[i] = true;
            }
        }

        public int CalculateHeuristic(GridSearchNode node)
        {
            return HeuristicFunction.Calc_H(this, node);
        }

        public bool IsBlocked(Location loc)
        {
            return _isBlockedLocations[loc.Y * Width + loc.X];
        }

        public BitArray GetBlockedOrVisited(GridSearchNode searchNode)
        {
            BitArray mergedArrays = new BitArray(_isBlockedLocations.Count);
            mergedArrays.Or(_isBlockedLocations);
            mergedArrays.Or(searchNode.Visited);
            return mergedArrays;
        }

        public T GetInitialSearchNode<T>() where T : GridSearchNode
        {
            return (T)Activator.CreateInstance(typeof(T), this);
        }
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