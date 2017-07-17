using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using MaSib.Algorithms;
using MaSib.Domain.SIB;

namespace MaSib
{
    public class Box : INode, IBox
    {
        public const int MAX_DIM = 8;
        [DebuggerDisplay("{this.GetBitsString(),nq}")]
        private IBoxHeuristic heuristicFunction;
        private int gValue;
        private int hValue;

        public World world { get; internal set; }
        public Snake[] snakes { get; internal set; }

        public Box(World world, int[] snakesStartLocations, IBoxHeuristic heuristicFunction, ISnakeHeuristic heuristicForSnakes)
        {
            this.world = world;
            snakes = new Snake[snakesStartLocations.Length];
            for (int i = 0; i < snakesStartLocations.Length; i++)
            {
                snakes[i] = new Snake(world, snakesStartLocations[i], heuristicForSnakes, i==0); //TODO: do we used imprune on one snake???
            }
            this.heuristicFunction = heuristicFunction;
            calculateGValue();
            hValue = this.heuristicFunction.calc_h(this);
        }

        private Box(World world, Snake[] snakes, IBoxHeuristic heuristicFunction) 
        {
            this.world = world;
            this.snakes = snakes;
            calculateGValue();
            this.heuristicFunction = heuristicFunction;
            hValue = this.heuristicFunction.calc_h(this);

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

        public LinkedList<INode> Children {
            get
            {
                var tmpList = CartesianProductOfSnakesChilds();
                tmpList = RemoveChildsByBoxSpread(tmpList);
                return RemoveDuplicationOfCurrentNode(tmpList);
            }
        }

        private LinkedList<INode> RemoveDuplicationOfCurrentNode(LinkedList<INode> boxList)  //TODO: maby not needed - need better solution
        {
            LinkedList<INode> result = new LinkedList<INode>();
            bool allSameSnakes;
            foreach (var box in boxList)
            {
                Box b = (Box)box;
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
                Box b = (Box)box;
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
                result.AddLast(new Box(world, snakes, heuristicFunction));  //parent = this 

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
            return String.Format("G:{0} Bits:{1}", g,GetBitsString());
        }


    }

}
