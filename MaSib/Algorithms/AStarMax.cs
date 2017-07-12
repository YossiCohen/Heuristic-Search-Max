using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace MaSib.Algorithms
{
    public enum State
    {
        Searching,Ended
    }

    public class AStarMax : ISolver
    {
        private INode candidateGoalNode;
        private SortedList<int, INode> openList;
//        private SortedList<int, INode> closedList;

        public int Expended { get; private set; }
        public int Generated { get; private set; }
        public int Pruned { get; private set; }

        public AStarMax(INode initailNode)
        {
            this.candidateGoalNode = initailNode;
            var duplicateComparer = new MaxComparer();
            openList = new SortedList<int, INode>(duplicateComparer);
            openList.Add(initailNode);
            Expended = 0;
            Generated = 0;
            Pruned = 0;  //Will stay 0 - just implementing ISolver
        }

        public State Run()
        {
            while (true)
            {
                State s = Step();
                if (s != State.Searching)
                    return s;
            }
        }

        public State Step()
        {
            // stop condition
            if (openList.IsEmpty())
            {
                return State.Ended;
            }
            // Check the next best node in OPEN and set it to current
            var currentNode = openList.Pop();

            //store best candidate if we seeing it
            if (currentNode.g > candidateGoalNode.g)
            {
                candidateGoalNode = currentNode;
                Log.WriteLineIf("AStar Best Candidate:" + candidateGoalNode, TraceLevel.Verbose);
            }

            if (currentNode.f < candidateGoalNode.g)
            {
                return State.Ended;
            }
            //Expand current node
            Expended++;
            foreach (var child in currentNode.Children)
            {
                openList.Add(child);
                Generated++;
            }
            return State.Searching;
        }

        public INode GetMaxGoal()
        {
            return candidateGoalNode;
        }


    }

}
