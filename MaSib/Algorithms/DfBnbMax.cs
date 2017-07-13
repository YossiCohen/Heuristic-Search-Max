using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace MaSib.Algorithms
{

    public class DfBnbMax : Solver
    {
        private Stack<INode> openList;

        public DfBnbMax(INode initailNode) : base(initailNode)
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
            if (currentNode.f < candidateGoalNode.g)
            {
                Pruned++;
                return State.Searching;
            }
            //Expand what is not pruned
            Expended++;
            //store best candidate if we seeing it
            if (currentNode.g > candidateGoalNode.g)
            {
                candidateGoalNode = currentNode;
                Log.WriteLineIf("DFBnB Best Candidate:" + candidateGoalNode, TraceLevel.Verbose);
            }

            foreach (var child in currentNode.Children)
            {
                openList.Push(child);
                Generated++;
            }

            return State.Searching;
        }

    }

}
