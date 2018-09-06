using System;
using System.Collections.Generic;
using System.Diagnostics;
using Common;

namespace MaxSearchAlg
{

    public class GreedyLoopMax : Solver
    {
        private static Random rnd = new Random();
        private INode _initialNode;
        private int _iterationsSinceLastImprovement;
        private int _lastBestG;
        private int _loops;

        public GreedyLoopMax(INode initialNode, IGoalCheckMethod goalCheckMethod, int loops) : base(initialNode, new NoPrunning(), goalCheckMethod)
        {
            var maxComparer = new MaxComparer();
            _initialNode = initialNode;
            _iterationsSinceLastImprovement = 0;
            _lastBestG = 0;
            _loops = loops;
        }


        internal override State Step()
        {
            Log.WriteLineIf($"[StepStart] Solving the Greedy way", TraceLevel.Verbose);

            Solver solver = new GreedyMax(_initialNode, GoalCheckMethod);
            solver.Run(15); //TODO: propogate? it's irrelevant in here
            Expended += solver.Expended;
            Generated += solver.Generated;
            if (solver.candidateGoalNode.g > _lastBestG)
            {
                _iterationsSinceLastImprovement = 0;
                _lastBestG = solver.candidateGoalNode.g;
                candidateGoalNode = solver.candidateGoalNode;
            }

            _iterationsSinceLastImprovement++;

            if (_iterationsSinceLastImprovement >= _loops)// stop condition
            {
                if (candidateGoalNode == null)
                {
                    Log.WriteLineIf("[GreedyLoops.StepEnd] candidateGoalNode == null - return Ended.NoGoalFound", TraceLevel.Verbose);
                    return State.NoGoalFound;
                }
                //Have we found the goal? - Greedy will check that :)
                Log.WriteLineIf("[GreedyLoops.StepEnd LoopsEnded] GoalFound:" + candidateGoalNode.GetBitsString(), TraceLevel.Verbose);
                return State.Ended;
            }
            Log.WriteLineIf($"[GreedyLoops.Status] Generated:{Generated}, Expended:{Expended}, IterSinceLast:{_iterationsSinceLastImprovement}, BestFoundG:{_lastBestG}, Loops:{_loops}", TraceLevel.Off);
            Log.WriteLineIf("[AStarMax.StepEnd] - return Searching", TraceLevel.Verbose);
            return State.Searching;
        }
    }
}
