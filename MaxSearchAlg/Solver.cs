using System;
using System.Diagnostics;
using Common;

namespace MaxSearchAlg
{
    public enum State
    {
        Searching, Ended, StoppedByTime, IllegalStartState, StoppedByMemoryLimit
    }

    public abstract class Solver
    {
        internal INode candidateGoalNode;
        internal IPrunningMethod PrunningMethod;
        internal IGoalCheckMethod GoalCheckMethod;
        internal static readonly int LogSearchStatusEveryXGenerated = 10000;

        public Solver(INode initailNode, IPrunningMethod prunningMethod, IGoalCheckMethod goalCheckMethod)
        {
            Expended = 0;
            Generated = 0;
            Pruned = 0;
            PrunningMethod = prunningMethod;
            GoalCheckMethod = goalCheckMethod;
            if (GoalCheckMethod.ValidGoal(initailNode))
            {
                candidateGoalNode = initailNode;
            }
        }
        public int Expended { get; internal set; }
        public int Generated { get; internal set; }
        public int Pruned { get; internal set; }

        /// <summary>
        /// Steps the solver algorithm forward until it finds the goal node or time is up
        /// </summary>
        /// <returns>Returns the state the algorithm finished in</returns>
        public State Run(int timelimit, int memoryLimit_MB = 0)
        {
            long memoryLimit = memoryLimit_MB * 1024 * 1024;  //B -> KB -> MB
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
                //Stop accorting to time limitation    
                if (timelimit != 0 && DateTime.Now.Subtract(startTime).TotalMinutes >= timelimit)
                {
                    return State.StoppedByTime;
                }

                //Stop accorting to memory limitation
                if (memoryLimit != 0 && Process.GetCurrentProcess().WorkingSet64 > memoryLimit)
                {
                    return State.StoppedByMemoryLimit;
                }

                //Periodic Log prints
                if (Generated % LogSearchStatusEveryXGenerated == 0)
                {
                    Log.WriteLineIf($"[SolverStatus] Generated:{Generated}, Expended:{Expended}, Pruned:{Pruned}, Time(min):{DateTime.Now.Subtract(startTime).TotalMinutes}, Memory:{Process.GetCurrentProcess().WorkingSet64}", TraceLevel.Verbose);
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
