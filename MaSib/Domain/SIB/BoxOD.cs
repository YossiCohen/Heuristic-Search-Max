using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using MaSib.Algorithms;
using MaSib.Domain.SIB;

namespace MaSib
{
    public class BoxOD : Box, INode
    {

        private int operatorIndex;

        public BoxOD(World world, int[] snakesStartLocations, IBoxHeuristic heuristicFunction, 
            ISnakeHeuristic heuristicForSnakes) : base(world, snakesStartLocations, heuristicFunction, heuristicForSnakes)
        {
            operatorIndex = 0;
        }

        private BoxOD(World world, Snake[] snakes, IBoxHeuristic heuristicFunction, int operatorIndex) : base(world, snakes, heuristicFunction)
        {
            this.operatorIndex = operatorIndex;
        }


        public override LinkedList<INode> Children {
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
                            if (World.HammingDistance(b.snakes[i].Head, b.snakes[j].tail[k]) < world.BoxSpread)
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


    }

}
