using System;
using System.Collections.Generic;
using System.Diagnostics;
using Common;

namespace MaxSearchAlg
{

    public class Greedy : Solver
    {
        private static Random rnd = new Random();
        private INode _head;

        public Greedy(INode initailNode, IGoalCheckMethod goalCheckMethod) : base(initailNode, new NoPrunning(), goalCheckMethod)
        {
            var maxComparer = new MaxComparer();
            _head = initailNode;
        }


        internal override State Step()
        {
            Log.WriteLineIf($"[StepStart] Head.G:{_head.g}, Head.H:{_head.h}", TraceLevel.Verbose);
            // stop condition
            if (_head.Children.Count == 0)
            {
                Log.WriteLineIf("[StepEnd] Head.Children.Count == 0 - return Ended-EmptyList", TraceLevel.Verbose);
                return State.Ended;
            }

            //Have we found the goal?
            if (GoalCheckMethod.ValidGoal(_head))
            {
                if (candidateGoalNode == null)
                {
                    Log.WriteLineIf("[AStarMax.StepEnd GoalFound] GoalFound:" + _head.GetBitsString(), TraceLevel.Verbose);
                    candidateGoalNode = _head;
                    return State.Ended;
                }
            }

            //Expand head
            Expended++;
            Log.WriteLineIf("[StepExpanding...] ", TraceLevel.Verbose);
            int best_h=-1;
            List<INode> sameHeuristicChilds = new List<INode>();
            foreach (var child in _head.Children)
            {
                Log.WriteLineIf($"[GenerateChild...] {child.GetBitsString()}", TraceLevel.Info);
                Generated++;
                if (child.h > best_h)
                {
                    best_h = child.h;
                    sameHeuristicChilds = new List<INode>();
                }
                if (child.h == best_h)
                {
                    sameHeuristicChilds.Add(child);
                }
            }
            _head = sameHeuristicChilds[rnd.Next(sameHeuristicChilds.Count)];

            Log.WriteLineIf("[AStarMax.StepEnd] - return Searching", TraceLevel.Verbose);
            return State.Searching;
        }
    }
}
