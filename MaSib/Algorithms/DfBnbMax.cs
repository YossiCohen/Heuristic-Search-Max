using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace MaSib.Algorithms
{

    public class DfBnbMax : ISolver
    {
        private INode candidateGoalNode;
        private Stack<INode> openList;

        public int Expended { get; private set; }
        public int Generated { get; private set; }
        public int Pruned { get; private set; }
        public DfBnbMax(INode initailNode)
        {
            this.candidateGoalNode = initailNode;   
            openList = new Stack<INode>();
            openList.Push(initailNode);
            Expended = 0;
            Generated = 0;
            Pruned = 0;
        }

        /// <summary>
        /// Steps the DFBnB algorithm forward until it finds the goal node.
        /// </summary>
        /// <returns>Returns the state the algorithm finished in</returns>
        public State Run()
        {
            // Continue searching until either failure or the goal node has been found.
            while (true)
            {
                State s = Step();
                if (s != State.Searching)
                    return s;
            }
        }

        /// <summary>
        /// Moves the DFBnB algorithm forward one step.
        /// </summary>
        /// <returns>Returns the state the alorithm is in after the step, Searching or Ended.</returns>
        public State Step()
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

        public INode GetMaxGoal()
        {
            return candidateGoalNode;
        }


    }

}
