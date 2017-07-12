using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using MaSib.Algorithms;
using MaSib.Domain.SIB;

namespace MaSib
{
    public class BoxOD : INode, IBox
    {
        public const int MAX_DIM = 8;
        [DebuggerDisplay("{this.GetBitsString(),nq}")]
        private IBoxHeuristic heuristicFunction;
        private int operatorIndex;
        private int gValue;
        private int hValue;

        public World world { get; internal set; }
        public Snake[] snakes { get; internal set; }

        public BoxOD(World world, int[] snakesStartLocations, IBoxHeuristic heuristicFunction, ISnakeHeuristic heuristicForSnakes)
        {
            this.world = world;
            snakes = new Snake[snakesStartLocations.Length];
            for (int i = 0; i < snakesStartLocations.Length; i++)
            {
                snakes[i] = new Snake(world, snakesStartLocations[i], heuristicForSnakes, i==0); //TODO: do we used imprune on one snake???
            }
            calculateGValue();
            this.heuristicFunction = heuristicFunction;
            hValue = this.heuristicFunction.calc_h(this);
            operatorIndex = 0;
        }

        private BoxOD(World world, Snake[] snakes, IBoxHeuristic heuristicFunction, int operatorIndex) //Box parent,
        {
            this.world = world;
//            this.Parent = parent;
            this.snakes = snakes;
            calculateGValue();
            this.heuristicFunction = heuristicFunction;
            hValue = this.heuristicFunction.calc_h(this);
            this.operatorIndex = operatorIndex;
        }

        private void calculateGValue()
        {
            gValue = 0;
            for (int i = 0; i < snakes.Length; i++)
            {
                gValue += snakes[i].g;
            }
        }
        public int NumberOfSnake {
            get { return snakes.Length; }
        }

        public int f
        {
            get { return g + h; }
        }
        public int h {
            get { return hValue; }
        }

        public int g
        {
            get { return gValue; }
        }
//        public INode Parent { get; set; }

        public LinkedList<INode> Children {
            get
            {
                var tmpList = GetChildrenByOperatorDecomposition();
                return RemoveChildsByBoxSpread(tmpList);
            }
        }

        private LinkedList<INode> RemoveChildsByBoxSpread(LinkedList<INode> boxList)
        {
            LinkedList<INode> result = new LinkedList<INode>();
            bool spreadFaultFound;
            foreach (var box in boxList)
            {
                BoxOD b = (BoxOD)box;
                spreadFaultFound = false;
                for (int i = 0; i < b.snakes.Length; i++)//i snake
                {
                    for (int j = 0; j < b.snakes.Length; j++)//j snake
                    {
                        if (i == j)
                        {
                            continue;
                        }
                        for (int k = 0; k < b.snakes[j].tail.Length; k++)
                        {
                            if (world.HammingDistance(b.snakes[i].Head, b.snakes[j].tail[k]) < world.BoxSpread)
                            {
                                spreadFaultFound = true;
                                break;
                            }

                        }
                        if (spreadFaultFound)
                        {
                            break;
                        }
                    }
                    if (spreadFaultFound)
                    {
                        break;
                    }
                }

                if (!spreadFaultFound)
                {
                    result.AddLast(b);
                }
            }
            return result;
        }

        private LinkedList<INode> GetChildrenByOperatorDecomposition()
        {
            LinkedList<INode> result = new LinkedList<INode>();
            var childs = snakes[operatorIndex].Children;
            foreach (var child in childs)
            {
                Snake[] s = new Snake[this.snakes.Length];
                for (int i = 0; i < this.snakes.Length; i++)
                {
                    if (i == operatorIndex)
                    {
                        s[i] = (Snake)child;
                    }
                    else
                    {
                        s[i] = this.snakes[i];
                    }
                }
                result.AddLast(new BoxOD(world, s, this.heuristicFunction,(this.operatorIndex + 1) % snakes.Length));
            }
            return result;
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
    }

}
