using System.Collections.Generic;
using System.Diagnostics;
using Common;

namespace MaxSearchAlg
{

    public class AStarMax : Solver
    {
        
        private SortedList<int, INode> openList;

        public AStarMax(INode initailNode, IGoalCheckMethod goalCheckMethod) : this(initailNode, new NoPrunning(), goalCheckMethod)
        {
        }

        public AStarMax(INode initailNode, IPrunningMethod prunningMethod, IGoalCheckMethod goalCheckMethod) :base(initailNode, prunningMethod, goalCheckMethod)
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

            if (Generated % LogSearchStatusEveryXGenerated == 0)
            {
                Log.WriteLineIf($"[AStarMax] OpenListSize:{openList.Count}", TraceLevel.Verbose);
            }

            //store best candidate if we seeing it
            if (GoalCheckMethod.ValidGoal(currentNode))
            {
                if (candidateGoalNode == null || currentNode.g > candidateGoalNode.g)
                {
                    candidateGoalNode = currentNode;
                    Log.WriteLineIf("[AStarMax] Best Candidate:" + candidateGoalNode, TraceLevel.Verbose);
                }
            }

            if (candidateGoalNode != null && currentNode.f < candidateGoalNode.g)
            {
                return State.Ended;
            }
            //Expand current node
            Expended++;
            foreach (var child in currentNode.Children)
            {
                if (!PrunningMethod.ShouldPrune(child))
                {
                    openList.Add(child);
                    Generated++;
                }
                else
                {
                    Pruned++;
                }

            }
            return State.Searching;
        }
    }
}
