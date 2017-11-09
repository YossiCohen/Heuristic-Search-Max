using System.Collections.Generic;
using System.Diagnostics;
using Common;

namespace MaxSearchAlg
{

    public class AStarMax : Solver
    {
        
        private SortedList<int, INode> openList;

        public AStarMax(INode initailNode):base(initailNode)
        {
            var duplicateComparer = new MaxComparer();
            openList = new SortedList<int, INode>(duplicateComparer);
            openList.Add(initailNode);
        }



        internal override State Step()
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




    }

}
