using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using MaSib.Algorithms;
using MaSib.Domain.SIB;

namespace MaSib
{
    public class Snake: ISibNode
    {
        private int head;
        private int visitedDim;
        internal World world;
        [DebuggerDisplay("{GetBitsString(),nq}")]
        public int[] tail;

        private ISnakeHeuristic heuristicFunction;
        private int hValue;
//        private LinkedList<INode> children;

        public Snake(World world, int head, ISnakeHeuristic heuristicFunction, bool useDim = true)
        {
            this.head = head;
            this.world = world;
            this.visitedDim = (useDim ?  0 : world.Dimentions);
            if (!world.ValidPosition(head))
            {
                throw new ArgumentException("Snake position is invalid");
            }
            this.tail = new int[1];
            this.tail[this.tail.Length-1] = head;
//            this.Parent = null;
            this.heuristicFunction = heuristicFunction;
            this.hValue = heuristicFunction.calc_h(this);
        }

        public Snake(Snake parent, int newHeadPosition, bool incDim)
        {
            this.head = newHeadPosition;
            this.world = parent.world;
            if (incDim)
            {
                if (parent.visitedDim == world.Dimentions)
                {
                    this.visitedDim = world.Dimentions;
                }
                else
                {
                    this.visitedDim = parent.visitedDim+1;
                }
            }
            else
            {
                this.visitedDim = parent.visitedDim;
            }
            if (!world.ValidPosition(head))
            {
                throw new ArgumentException("Snake position is invalid");
            }
            this.tail = new int[parent.tail.Length+1];
            for (int i = 0; i < parent.tail.Length; i++)
            {
                this.tail[i] = parent.tail[i];
            }
            this.tail[tail.Length - 1] = head;
//            this.Parent = parent;
            this.heuristicFunction = parent.heuristicFunction;
            this.hValue = heuristicFunction.calc_h(this);
        }

        public int g
        {
            get { return tail.Length-1; }
        }

//        public INode Parent { get; set; }

        public int h
        {
            get { return this.hValue; }
        }

        public int f
        {
            get { return g + h; }
        }

        public int VisitedDim
        {
            get { return visitedDim; }
        }

        public int Head
        {
            get { return head; }
        }

        public LinkedList<INode> Children
        { 
            get
            {
                var result = generateInitialChildList();
                result = removeChildsThatExistsInBody(result);
                result = removeChildsWithBadHeadSpread(result);
                return result;
            }
        }

        private LinkedList<INode> removeChildsWithBadHeadSpread(LinkedList<INode> childs)
        {
            LinkedList<INode> result = new LinkedList<INode>();
            bool found;
            foreach (var child in childs)
            {
                Snake c = (Snake)child;
                found = false;
                for (int i = 0; i < c.tail.Length - world.SnakeSpread; i++) 
                {
                    if (World.HammingDistance(c.Head, c.tail[i]) < world.SnakeSpread)
                    {
                        found = true;
                        break;
                    }

                }
                if (!found)
                {
                    result.AddLast(c);
                }
            }
            return result;
        }

        private LinkedList<INode> removeChildsThatExistsInBody(LinkedList<INode> childs)
        {
            LinkedList<INode> result = new LinkedList<INode>();
            bool found;
            foreach (var child in childs)
            {
                Snake c = (Snake)child;
                found = false;
                for (int i = 0; i<c.tail.Length-1;i++)
                {
                    if (c.head == c.tail[i])
                    {
                        found = true;
                        break;
                    }

                }
                if (!found)
                {
                    result.AddLast(c);
                }
            }
            return result;
        }

        private LinkedList<INode> generateInitialChildList()
        {
            
            LinkedList<INode> result = new LinkedList<INode>();
            var loopMax = Math.Min(visitedDim, world.Dimentions);
            int newHeadPosition;
            for (int i = 0; i < visitedDim; i++)
            {
                newHeadPosition = FlipBitAt(head, i);
                if (this.g > 1)
                {
                    if (newHeadPosition == this.tail[this.tail.Length - 2])  //((Snake)this.Parent).head)
                    {
                        continue; //Snake not growing into itself
                    }
                }
                result.AddLast(new Snake(this, newHeadPosition, false));
            }

            if (visitedDim < world.Dimentions)
            {
                newHeadPosition = FlipBitAt(head, visitedDim);
                result.AddLast(new Snake(this, newHeadPosition, true));
            }
            return result;
        }


        public static bool ValidOneStepOfSnake(int a, int b)
        {
            return World.HammingDistance(a,b) == 1;
        }

        public static int FlipBitAt(int source, int idx)
        {
            var result = source ^ (1 << idx);
            return result;
        }

        public override string ToString()
        {
            return String.Format("G:{0} Bits:{1}",tail.Length, GetBitsString());
        }

        public string GetBitsString()
        {
            return world.IntArrToBitString(tail);
        }

        public string GetIntString()
        {
            StringBuilder sb = new StringBuilder();
            string prefix = "";
            foreach (var i in tail)
            {
                sb.Append(prefix);
                prefix = "-";
                sb.Append(i);
            }
            return sb.ToString();
        }

        public List<int> GetSnakeSpreadFreeSpots()
        {
            List<int> freeSpots = new List<int>();
            for (int i = 0; i < world.MaxPlacesInDimention; i++)
            {
                bool valid = true;
                foreach (var part in tail)
                {
                    if (World.HammingDistance(i, part) < world.SnakeSpread)
                    {
                        valid = false;
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
