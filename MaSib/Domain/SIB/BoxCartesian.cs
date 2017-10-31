using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using MaSib.Domain.SIB;
using MaxSearchAlg;

namespace MaSib
{
    public class BoxCartesian : Box
    {

        public BoxCartesian(World world, int[] snakesStartLocations, IBoxHeuristic heuristicFunction, 
            ISnakeHeuristic heuristicForSnakes) : base(world, snakesStartLocations, heuristicFunction, heuristicForSnakes)
        {
        }

        private BoxCartesian(World world, Snake[] snakes, IBoxHeuristic heuristicFunction)
            : base(world, snakes, heuristicFunction)
        {
        }

        public override LinkedList<INode>  Children {
            get
            {
                var tmpList = CartesianProductOfSnakesChilds();
                tmpList = RemoveChildsByBoxSpread(tmpList);
                tmpList = RemoveByLengthConstraint(tmpList);
                tmpList = RemoveDuplicationOfCurrentNode(tmpList);
                return tmpList;
            }
        }

        private LinkedList<INode> RemoveByLengthConstraint(LinkedList<INode> boxList)
        {
            LinkedList<INode> result = new LinkedList<INode>();
            int maxLength;
            int minLength;
            foreach (var box in boxList)
            {
                BoxCartesian b = (BoxCartesian)box;
                maxLength = Int32.MinValue;
                minLength = Int32.MaxValue;
                for (int i = 0; i < b.snakes.Length; i++)//i snake
                {
                    maxLength = Math.Max(maxLength, b.snakes[i].tail.Length);
                    minLength = Math.Min(minLength, b.snakes[i].tail.Length);
                }

                if (maxLength - minLength <= 1)
                {
                    result.AddLast(b);
                }
            }
            return result;
        }

        private LinkedList<INode> RemoveDuplicationOfCurrentNode(LinkedList<INode> boxList)
        {
            LinkedList<INode> result = new LinkedList<INode>();
            bool allSameSnakes;
            foreach (var box in boxList)
            {
                BoxCartesian b = (BoxCartesian)box;
                allSameSnakes = true;
                for (int i = 0; i < b.snakes.Length; i++)//i snake
                {

                    if (this.snakes[i]!=b.snakes[i])
                    {
                        allSameSnakes = false;
                        break;
                    }
                }

                if (!allSameSnakes)
                {
                    result.AddLast(b);
                }
            }
            return result;
        }

        private LinkedList<INode> RemoveChildsByBoxSpread(LinkedList<INode> boxList)
        {
            LinkedList<INode> result = new LinkedList<INode>();
            bool spreadFaultFound;
            foreach (var box in boxList)
            {
                BoxCartesian b = (BoxCartesian)box;
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

        private LinkedList<INode> CartesianProductOfSnakesChilds()
        {
            List<List<INode>> allSnakesChildes = new List<List<INode>>();
            LinkedList<INode> result = new LinkedList<INode>();
            var sum_childs = 0; //This will help to check if there are no childs to the snakes AT ALL
            foreach (var snake in snakes)
            {
                var childrenList = snake.Children.ToList();
                sum_childs += childrenList.Count;
                childrenList.Add(snake); //One child is the non moving snake to allow imbalanced results (Meeting with Roni - Disable wait moves should remove this but it will make cartesian wrong)
                allSnakesChildes.Add(childrenList);
            }
            if (sum_childs == 0)
            {
                return result;  //If all snakes have no children - stop 
                // this will ignore the single snakes list to prevent infinite loop
            }

            int listCount = allSnakesChildes.Count;
            List<int> indexes = new List<int>();
            for (int i = 0; i < listCount; i++)
                indexes.Add(0);

            while (true)
            {
                // construct values
                INode[] values = new INode[listCount];
                for (int i = 0; i < listCount; i++)
                    values[i] = allSnakesChildes[i][indexes[i]];

                Snake[] snakes = Array.ConvertAll(values, prop => (Snake) prop);
                result.AddLast(new BoxCartesian(world, snakes, heuristicFunction)); 

                // increment indexes
                int incrementIndex = listCount - 1;
                while (incrementIndex >= 0 && ++indexes[incrementIndex] >= allSnakesChildes[incrementIndex].Count)
                {
                    indexes[incrementIndex] = 0;
                    incrementIndex--;
                }

                // break condition
                if (incrementIndex < 0)
                    break;
            }
            return result;
        }
        
    }

}
