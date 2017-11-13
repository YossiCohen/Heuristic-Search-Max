using System.Collections;
using System.Collections.Generic;
using MaxSearchAlg;

namespace Grid.Domain
{
    public class ReachableSymmetryDetectionPrunning : IPrunningMethod
    {
        private Dictionary<long, List<GridSearchNode>> HistoryNodes;

        public ReachableSymmetryDetectionPrunning()
        {
            HistoryNodes = new Dictionary<long, List<GridSearchNode>>();
        }

        public bool ShouldPrune(INode node)
        {
            var newGridNode = node as GridSearchNode;
            var hash = GetLinearHeadLocation(newGridNode);
            if (!HistoryNodes.ContainsKey(hash))
            {
                HistoryNodes[hash] = new List<GridSearchNode>();
                HistoryNodes[hash].Add(newGridNode);
                return false;
            }
            else
            {
                var relevantList = HistoryNodes[hash];
                foreach (var historyNode in relevantList)
                {
                    if (EqualBitArray(historyNode.Visited,newGridNode.Visited))
                    {
                        return true;
                    }
                }
                relevantList.Add(newGridNode);
                return false;
            }
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
