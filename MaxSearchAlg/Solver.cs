using System;

namespace MaxSearchAlg
{
    public enum State
    {
        Searching, Ended, StoppedByTime, IllegalStartState
    }

    public abstract class Solver
    {

        internal INode candidateGoalNode;
        internal IPrunningMethod PrunningMethod;

        public Solver(INode initailNode, IPrunningMethod prunningMethod)
        {
            this.candidateGoalNode = initailNode;
            Expended = 0;
            Generated = 0;
            Pruned = 0;
            PrunningMethod = prunningMethod;
        }
        public int Expended { get; internal set; }
        public int Generated { get; internal set; }
        public int Pruned { get; internal set; }

        /// <summary>
        /// Steps the solver algorithm forward until it finds the goal node or time is up
        /// </summary>
        /// <returns>Returns the state the algorithm finished in</returns>
        public State Run(int timelimit)
        {
            var startTime = DateTime.Now;
            while (true)
            {
                State s = Step();
                if (s != State.Searching)
                {
                    if (Expended == 1) //Only one expand and not continued searching?
                    {
                        return State.IllegalStartState;
                    }
                    else
                    {
                        return s;
                    }
                }
                    
                if (timelimit != 0 && DateTime.Now.Subtract(startTime).TotalMinutes >= timelimit)
                {
                    return State.StoppedByTime;
                }
            }
        }

        internal abstract State Step();

        public INode GetMaxGoal()
        {
            return candidateGoalNode;
        }
    }
}
