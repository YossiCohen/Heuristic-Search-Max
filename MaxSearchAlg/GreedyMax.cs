using System;
using System.Collections.Generic;
using System.Diagnostics;
using Common;

namespace MaxSearchAlg
{

    public class GreedyMax : Solver
    {
        private static Random rnd = new Random();
        private INode _head;

        public GreedyMax(INode initialNode, IGoalCheckMethod goalCheckMethod) : base(initialNode, new NoPrunning(), goalCheckMethod)
        {
            var maxComparer = new MaxComparer();
            _head = initialNode;
        }


        internal override State Step()
        {
            Log.WriteLineIf($"[StepStart] Head.G:{_head.g}, Head.H:{_head.h}", TraceLevel.Verbose);


            //Have we found the goal?
            if (GoalCheckMethod.ValidGoal(_head))
            {
                if (candidateGoalNode == null)
                {
                    Log.WriteLineIf("[GreedyMax.StepEnd GoalFound] GoalFound:" + _head.GetBitsString(), TraceLevel.Verbose);
                    candidateGoalNode = _head;
                    return State.Ended;
                }
            }

            // stop condition
            if (_head.Children.Count == 0)
            {
                Log.WriteLineIf("[StepEnd] Head.Children.Count == 0 - return Ended-NoGoalFound", TraceLevel.Verbose);
                return State.NoGoalFound;
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
            if (best_h != 0)
            {
                _head = sameHeuristicChilds[rnd.Next(sameHeuristicChilds.Count)];
            }
            else
            {
                for (int i = 0; i < sameHeuristicChilds.Count; i++)
                {
                    if (GoalCheckMethod.ValidGoal(sameHeuristicChilds[i]))
                    {
                        _head = sameHeuristicChilds[i];
                        break;
                    }
                }
            }

            Log.WriteLineIf("[GreedyMax.StepEnd] - return Searching", TraceLevel.Verbose);
            return State.Searching;
        }
    }
}
