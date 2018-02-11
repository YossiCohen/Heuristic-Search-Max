using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Common;
using MaxSearchAlg;

namespace Grid.Domain
{
    public class BasicSymmetryDetectionPrunning : IPrunningMethod
    {
        private Dictionary<long, List<GridSearchNode>> HistoryNodes;
        public BasicSymmetryDetectionPrunning()
        {
            HistoryNodes = new Dictionary<long, List<GridSearchNode>>();
        }

        public bool ShouldPrune(INode node)
        {
            var newGridNode = node as GridSearchNode;
            Log.WriteLineIf($"[ShouldPrune - new node] {newGridNode.GetBitsString()}", TraceLevel.Info);
            var hash = GetLinearHeadLocation(newGridNode);
            if (!HistoryNodes.ContainsKey(hash))
            {
                HistoryNodes[hash] = new List<GridSearchNode>();
                HistoryNodes[hash].Add(newGridNode);
                Log.WriteLineIf("[ShouldPrune - No list] - creating new", TraceLevel.Info);
            }
            else
            {
                var relevantList = HistoryNodes[hash];
                foreach (var historyNode in relevantList)
                {
                    Log.WriteLineIf($"[ShouldPrune] - checking history node{historyNode.GetBitsString()}", TraceLevel.Info);
                    if (EqualBitArray(historyNode.Visited,newGridNode.Visited))
                    {
                        Log.WriteLineIf("[ShouldPrune]- true", TraceLevel.Info);
                        return true;
                    }
                }
                relevantList.Add(newGridNode);
            }
            Log.WriteLineIf("[ShouldPrune] - false", TraceLevel.Info);
            return false;
        }

        private long GetLinearHeadLocation(GridSearchNode node)
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
    }
}
