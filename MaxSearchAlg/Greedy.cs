using System.Collections.Generic;
using System.Diagnostics;
using Common;

namespace MaxSearchAlg
{

    public class Greedy : Solver
    {
        INode head;

        public Greedy(INode initailNode, IGoalCheckMethod goalCheckMethod) : this(initailNode, new NoPrunning(), goalCheckMethod)
        {
        }

        public Greedy(INode initailNode, IPrunningMethod prunningMethod, IGoalCheckMethod goalCheckMethod) :base(initailNode, prunningMethod, goalCheckMethod)
        {
            var maxComparer = new MaxComparer();
            head = initailNode;
        }

        internal override State Step()
        {
            Log.WriteLineIf($"[StepStart] Open.Count:{OpenList.Count}, Exp:{Expended}, Gen:{Generated}, Prune:{Pruned}", TraceLevel.Verbose);
            // stop condition
            if (openList.IsEmpty())
            {
                Log.WriteLineIf($"[StepEnd] Open.Count:{OpenList.Count}, Exp:{Expended}, Gen:{Generated}, Prune:{Pruned} - return Ended-EmptyList", TraceLevel.Verbose);
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
                    Log.WriteLineIf("[ValidCandidate] Best Candidate:" + candidateGoalNode, TraceLevel.Verbose);
                }
            }

            if (candidateGoalNode != null && currentNode.f < candidateGoalNode.g)
            {
                Log.WriteLineIf($"[BestCandidate] Open.Count:{OpenList.Count}, Exp:{Expended}, Gen:{Generated}, Prune:{Pruned} - return Ended-BestFound!!!", TraceLevel.Verbose);
                return State.Ended;
            }
            //Expand current node
            Expended++;
            Log.WriteLineIf("[StepExpanding...] ", TraceLevel.Verbose);
            foreach (var child in currentNode.Children)
            {
#if DEBUG
                if (child.h > currentNode.h)
                {
                    Log.WriteLineIf($"[HEURISTIC NOT CONSISTENT] Parent: {currentNode.h}, Child:{child.GetBitsString()}", TraceLevel.Error);
                }
#endif
                Log.WriteLineIf($"[GenerateChild...] {child.GetBitsString()}", TraceLevel.Info);
                if (!PrunningMethod.ShouldPrune(child))
                {
                    Log.WriteLineIf("[Prune] false", TraceLevel.Verbose);
                    openList.Add(child);
                    Generated++;
                }
                else
                {
                    Log.WriteLineIf("[Prune] true", TraceLevel.Verbose);
                    Pruned++;
                }
            }
            Log.WriteLineIf($"[AStarMax.StepEnd] Open.Count:{OpenList.Count}, Exp:{Expended}, Gen:{Generated}, Prune:{Pruned} - return Searching", TraceLevel.Verbose);
            return State.Searching;
        }
    }
}
