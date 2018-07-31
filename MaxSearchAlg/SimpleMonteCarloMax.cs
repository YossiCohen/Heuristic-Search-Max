using System.Collections.Generic;
using System.Diagnostics;
using Common;

namespace MaxSearchAlg
{

    public class SimpleMonteCarloMax : Solver
    {
        private INode _frontier;

        public SimpleMonteCarloMax(INode initailNode, IGoalCheckMethod goalCheckMethod) : base(initailNode, new NoPrunning(), goalCheckMethod)
        {
            //TODO: Add SimNum,Add depth , Add Utility function
            _frontier = initailNode;
        }

        /// <summary>
        /// Moves the Monte Carlo algorithm forward one step.
        /// </summary>
        /// <returns>Returns the state the alorithm is in after the step, Searching or Ended.</returns>
        internal override State Step()
        {



            // Check the frontier for valid goal
            if (GoalCheckMethod.ValidGoal(_frontier))
            {
                candidateGoalNode = _frontier;
                Log.WriteLineIf("[SimpleMonteCarloMax] Goal Found:" + candidateGoalNode, TraceLevel.Verbose);
                return State.Ended;
            }

            //TODO: get legal childrens
            //TODO: no legal childrens = return State.Ended



            foreach (var child in _frontier.Children)
            {
                //TODO: Simulate until .depth 
            }

            //TODO: select best child and update frontier

            return State.Ended; //TODO: this is set to ended so tests wount stuck - change it to searching
        }

    }

}
