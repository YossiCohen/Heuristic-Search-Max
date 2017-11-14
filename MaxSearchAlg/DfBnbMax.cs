using System.Collections.Generic;
using System.Diagnostics;
using Common;

namespace MaxSearchAlg
{

    public class DfBnbMax : Solver
    {
        private Stack<INode> openList;

        public DfBnbMax(INode initailNode, IGoalCheckMethod goalCheckMethod) : this(initailNode, new NoPrunning(), goalCheckMethod)
        {
        }

        public DfBnbMax(INode initailNode, IPrunningMethod prunningMethod, IGoalCheckMethod goalCheckMethod) : base(initailNode, prunningMethod, goalCheckMethod)
        {
            openList = new Stack<INode>();
            openList.Push(initailNode);
        }

        /// <summary>
        /// Moves the DFBnB algorithm forward one step.
        /// </summary>
        /// <returns>Returns the state the alorithm is in after the step, Searching or Ended.</returns>
        internal override State Step()
        {
            // stop condition
            if (openList.IsEmpty())
            {
                return State.Ended;
            }
            // Check the next node in the queue and pop it
            var currentNode = openList.Pop();
            //do Prune if needed
            if (candidateGoalNode != null && currentNode.f < candidateGoalNode.g)
            {
                Pruned++;
                return State.Searching;
            }
            //Expand what is not pruned
            Expended++;
            //store best candidate if we seeing it
            if (GoalCheckMethod.ValidGoal(currentNode))
            {
                if (candidateGoalNode == null || currentNode.g > candidateGoalNode.g)
                {
                    candidateGoalNode = currentNode;
                    Log.WriteLineIf("[AStarMax] Best Candidate:" + candidateGoalNode, TraceLevel.Verbose);
                }
            }

            foreach (var child in currentNode.Children)
            {
                if (!PrunningMethod.ShouldPrune(child))
                {
                    openList.Push(child);
                    Generated++;
                }
                else
                {
                    Pruned++;//TODO: should separate the prune counter - BnB vs. PruningMethod
                }

            }

            return State.Searching;
        }

    }

}
