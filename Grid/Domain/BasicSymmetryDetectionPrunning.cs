using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Common;
using MaxSearchAlg;

namespace Grid.Domain
{
    public class BasicSymmetryDetectionPrunning : IPrunningMethod
    {
        private Dictionary<int, List<GridSearchNode>> HistoryNodes;
        public BasicSymmetryDetectionPrunning()
        {
            HistoryNodes = new Dictionary<int, List<GridSearchNode>>();
        }

        public bool ShouldPrune(INode node)
        {
            var newGridNode = node as GridSearchNode;
#if DEBUG
            Log.WriteLineIf($"[ShouldPrune - new node] {newGridNode.GetBitsString()}", TraceLevel.Info);
#endif
            var head = GetLinearHeadLocation(newGridNode);
            if (!HistoryNodes.ContainsKey(head))
            {
                HistoryNodes[head] = new List<GridSearchNode>();
                HistoryNodes[head].Add(newGridNode);
#if DEBUG
                Log.WriteLineIf("[ShouldPrune - No list] - creating new", TraceLevel.Info);
#endif
            }
            else
            {
                var relevantList = HistoryNodes[head];
                foreach (var historyNode in relevantList)
                {
#if DEBUG
                    Log.WriteLineIf($"[ShouldPrune] - checking history node{historyNode.GetBitsString()}", TraceLevel.Info);
#endif
                    if (EqualBitArray(historyNode.Visited,newGridNode.Visited))
                    {
#if DEBUG
                        Log.WriteLineIf("[ShouldPrune]- true", TraceLevel.Info);
#endif
                        return true;
                    }
                }
                relevantList.Add(newGridNode);
            }
#if DEBUG
            Log.WriteLineIf("[ShouldPrune] - false", TraceLevel.Info);
#endif
            return false;
        }

        private int GetLinearHeadLocation(GridSearchNode node)
        {
            return node.HeadLocation.Y * node.World.Width + node.HeadLocation.X;
        }

        private bool EqualBitArray(BitArray a, BitArray b)
        {
            int finalIndex = a.Length - 1;
            for (int i = 0; i < finalIndex; i++)
            {
                if (a[i] != b[i])
                {
                    return false;
                }
            }
            return true;
        }

        public string GetName()
        {
            return "B BSD_Pr";
        }
    }
}
