using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using MaSib.Algorithms;
using MaSib.Domain.SIB;

namespace MaSib
{
    public class BoxCartez : Box
    {

        public BoxCartez(World world, int[] snakesStartLocations, IBoxHeuristic heuristicFunction, 
            ISnakeHeuristic heuristicForSnakes) : base(world, snakesStartLocations, heuristicFunction, heuristicForSnakes)
        {
        }

        private BoxCartez(World world, Snake[] snakes, IBoxHeuristic heuristicFunction)
            : base(world, snakes, heuristicFunction)
        {
        }

        public override LinkedList<INode>  Children {
            get
            {
                var tmpList = CartesianProductOfSnakesChilds();
                tmpList = RemoveChildsByBoxSpread(tmpList);
                return RemoveDuplicationOfCurrentNode(tmpList);
            }
        }

        private LinkedList<INode> RemoveDuplicationOfCurrentNode(LinkedList<INode> boxList)  //TODO: maybe not needed - need better solution
        {
            LinkedList<INode> result = new LinkedList<INode>();
            bool allSameSnakes;
            foreach (var box in boxList)
            {
                BoxCartez b = (BoxCartez)box;
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
                BoxCartez b = (BoxCartez)box;
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
                if (childrenList.Count == 0)
                {
                    childrenList.Add(snake); //One child is the non moving snake to allow imbalanced results
                }
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
                result.AddLast(new BoxCartez(world, snakes, heuristicFunction));  //parent = this 

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
