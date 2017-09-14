using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Facet.Combinatorics;
using MaSib.Algorithms;
using MaSib.Domain.SIB;

namespace MaSib
{
    public class BoxVirtualNode<T> : ISibNode
    {
        private int numberOfSnakes;
        private ISnakeHeuristic heuristicForSnakes;
        private IBoxHeuristic heuristicFunction;
        public World world { get; internal set; }

        public BoxVirtualNode(World world, int numOfSnakes, IBoxHeuristic heuristicFunction,
            ISnakeHeuristic heuristicForSnakes)
        {
            if (typeof(T) != typeof(BoxOD) && typeof(T) != typeof(BoxCartesian))
            {
                throw new ArgumentException("Bad type for BoxVirtualNode");
            }
            this.world = world;
            f = 0;
            h = Int32.MinValue;
            g = 0;
            numberOfSnakes = numOfSnakes;
            this.heuristicFunction = heuristicFunction;
            this.heuristicForSnakes = heuristicForSnakes;
        }
        public int f { get; }
        public int h { get; }
        public int g { get; }

        public LinkedList<INode> Children
        {
            get
            {
                var returnValue = new LinkedList<INode>();
                //remember, snake s0 location is always 0
                var possibleLocations = Enumerable.Range(1, world.MaxPlacesInDimention-1).ToArray();
                var snakesCombinations = new Combinations<int>(possibleLocations, numberOfSnakes-1); //No combination for first snake
                Box child;
                foreach (var comb in snakesCombinations)
                {
                    int[] snakesStartLocations = new int[comb.Count+1];
                    snakesStartLocations[0] = 0;
                    comb.CopyTo(snakesStartLocations, 1);
                    if (typeof(T) != typeof(BoxOD))
                    {
                        child = new BoxOD(world, snakesStartLocations, heuristicFunction, heuristicForSnakes);
                    } else if (typeof(T) != typeof(BoxCartesian))
                    {
                        child = new BoxCartesian(world, snakesStartLocations, heuristicFunction, heuristicForSnakes);
                    }
                    else
                    {
                        throw new ArgumentException("Bad box type");
                    }
                    returnValue.AddLast(child);
                }
                return returnValue;
            }
        }
        public string GetBitsString()
        {
            return "BoxVirtualNode";
        }

        public string GetIntString()
        {
            return "BoxVirtualNode";
        }

        public List<int> GetSnakeSpreadFreeSpots()
        {
            return new List<int>();
        }
    }

}
