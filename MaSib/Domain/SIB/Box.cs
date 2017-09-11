using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaSib.Algorithms;

namespace MaSib.Domain.SIB
{
    public abstract class Box : ISibNode
    {
        public World world { get; internal set;}
        public Snake[] snakes { get; internal set; }
        protected int gValue;
        protected int hValue;
        [DebuggerDisplay("{this.GetBitsString(),nq}")]
        protected IBoxHeuristic heuristicFunction;

        public Box(World world, int[] snakesStartLocations, IBoxHeuristic heuristicFunction,
            ISnakeHeuristic heuristicForSnakes)
        {
            this.world = world;
            snakes = new Snake[snakesStartLocations.Length];
            for (int i = 0; i < snakesStartLocations.Length; i++)
            {
                snakes[i] = new Snake(world, snakesStartLocations[i], heuristicForSnakes, i == 0); //TODO: do we used imprune on one snake???
            }
            this.heuristicFunction = heuristicFunction;
            hValue = this.heuristicFunction.calc_h(this);
            calculateGValue();
        }
        public Box(World world, Snake[] snakes, IBoxHeuristic heuristicFunction)
        {
            this.world = world;
            this.snakes = snakes;
            calculateGValue();
            this.heuristicFunction = heuristicFunction;
            hValue = this.heuristicFunction.calc_h(this);

        }

        protected void calculateGValue()
        {
            gValue = Int32.MaxValue;
            for (int i = 0; i < snakes.Length; i++)
            {
                gValue = Math.Min(snakes[i].g, gValue);
            }
        }

        public int NumberOfSnake
        {
            get { return snakes.Length; }
        }

        public abstract LinkedList<INode> Children { get; }

        public int f
        {
            get { return g + h; }
        }
        public int h
        {
            get { return hValue; }
        }

        public int g
        {
            get { return gValue; }
        }

        public string GetBitsString()
        {
            StringBuilder sb = new StringBuilder("|");
            foreach (var snake in snakes)
            {
                sb.Append(snake.GetBitsString());
                sb.Append("|");
            }
            return sb.ToString();
        }

        public string GetIntString()
        {
            StringBuilder sb = new StringBuilder("|");
            foreach (var snake in snakes)
            {
                sb.Append(snake.GetIntString());
                sb.Append("|");
            }
            return sb.ToString();
        }

        public override string ToString()
        {
            return String.Format("G:{0} Bits:{1}", g, GetBitsString());
        }


        public List<int> GetSnakeSpreadFreeSpots()
        {
            List<int> freeSpots = new List<int>();
            for (int i = 0; i < world.MaxPlacesInDimention; i++)
            {
                bool valid = true;
                foreach (var snake in snakes)
                {
                    foreach (var part in snake.tail)
                    {
                        if (World.HammingDistance(i, part) < world.SnakeSpread)
                        {
                            valid = false;
                            break;
                        }
                    }
                    if (!valid)
                    {
                        break;
                    }
                }

                if (valid)
                {
                    freeSpots.Add(i);
                }

            }
            return freeSpots;
        }

        public List<int> GetBoxSpreadFreeSpots()
        {
            List<int> freeSpots = new List<int>();
            for (int i = 0; i < world.MaxPlacesInDimention; i++)
            {
                bool valid = true;
                foreach (var snake in snakes)
                {
                    foreach (var part in snake.tail)
                    {
                        if (World.HammingDistance(i, part) < world.BoxSpread)
                        {
                            valid = false;
                            break;
                        }
                    }
                    if (!valid)
                    {
                        break;
                    }
                }

                if (valid)
                {
                    freeSpots.Add(i);
                }

            }
            return freeSpots;
        }
    }
}
