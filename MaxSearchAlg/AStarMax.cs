using System.Collections.Generic;
using System.Diagnostics;
using Common;

namespace MaxSearchAlg
{

    public class AStarMax : Solver
    {
        
        private SortedList<int, INode> openList;

        public AStarMax(INode initialNode, IGoalCheckMethod goalCheckMethod) : this(initialNode, new NoPrunning(), goalCheckMethod)
        {
        }

        public AStarMax(INode initialNode, IPrunningMethod prunningMethod, IGoalCheckMethod goalCheckMethod) :base(initialNode, prunningMethod, goalCheckMethod)
        {
            var maxComparer = new MaxComparer();
            openList = new SortedList<int, INode>(maxComparer);
            openList.Add(initialNode);
        }

        public SortedList<int, INode> OpenList
        {
            get { return openList; }
        }

        internal override State Step()
        {
#if DEBUG
            Log.WriteLineIf($"[StepStart] Open.Count:{OpenList.Count}, Exp:{Expended}, Gen:{Generated}, AlgPruned:{AlgPruned}, ExternalPruned:{ExternalPruned}", TraceLevel.Verbose);
#endif
            // stop condition
            if (openList.IsEmpty())
            {
#if DEBUG
                Log.WriteLineIf($"[StepEnd] Open.Count:{OpenList.Count}, Exp:{Expended}, Gen:{Generated}, AlgPruned:{AlgPruned}, ExternalPruned:{ExternalPruned} - return Ended-EmptyList", TraceLevel.Verbose);
#endif
                return State.Ended;
            }
            // Check the next best node in OPEN and set it to current
            var currentNode = openList.Pop();

            if (Generated % LogSearchStatusEveryXGenerated == 0)
            {
#if DEBUG
                Log.WriteLineIf($"[AStarMax] OpenListSize:{openList.Count}", TraceLevel.Verbose);
#endif
            }

            //store best candidate if we seeing it
            if (GoalCheckMethod.ValidGoal(currentNode)) ///TODO: if this is a valid goal - dont try to expand id
            {
                if (candidateGoalNode == null || currentNode.g > candidateGoalNode.g)
                {
                    candidateGoalNode = currentNode;
#if DEBUG
                    Log.WriteLineIf("[ValidCandidate] Best Candidate:" + candidateGoalNode, TraceLevel.Verbose);
#endif
                }
            }

            if (candidateGoalNode != null && currentNode.f < candidateGoalNode.g)
            {
#if DEBUG
                Log.WriteLineIf($"[BestCandidate] Open.Count:{OpenList.Count}, Exp:{Expended}, Gen:{Generated}, AlgPruned:{AlgPruned}, ExternalPruned:{ExternalPruned} - return Ended-BestFound!!!", TraceLevel.Verbose);
#endif
                return State.Ended;
            }
            //Expand current node
            Expended++;
#if DEBUG
            Log.WriteLineIf("[StepExpanding...] ", TraceLevel.Verbose);
#endif
            foreach (var child in currentNode.Children)
            {
#if DEBUG
                if (child.h > currentNode.h)
                {
                    Log.WriteLineIf($"[HEURISTIC NOT CONSISTENT] Parent: {currentNode.h}, Child:{child.GetBitsString()}", TraceLevel.Error);
                }
#endif
#if DEBUG
                Log.WriteLineIf($"[GenerateChild...] {child.GetBitsString()}", TraceLevel.Info);
#endif
                if (!PrunningMethod.ShouldPrune(child))
                {
#if DEBUG
                    Log.WriteLineIf("[Prune] false", TraceLevel.Verbose);
#endif
                    openList.Add(child);
                    Generated++;
                }
                else
                {
#if DEBUG
                    Log.WriteLineIf("[Prune] true", TraceLevel.Verbose);
#endif
                    ExternalPruned++;
                }
            }
#if DEBUG
            Log.WriteLineIf($"[AStarMax.StepEnd] Open.Count:{OpenList.Count}, Exp:{Expended}, Gen:{Generated}, AlgPruned:{AlgPruned}, ExternalPruned:{ExternalPruned} - return Searching", TraceLevel.Verbose);
#endif
            return State.Searching;
        }
    }
}
