using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using MaxSearchAlg;

namespace Grid.Domain
{
    public class ReachableSymmetryDetectionPrunning : IPrunningMethod
    {
        private Dictionary<long, List<RsdGridSearchNode>> HistoryNodes;
        private SortedList<int, INode> aStarOpenList;

        public ReachableSymmetryDetectionPrunning()
        {
            HistoryNodes = new Dictionary<long, List<RsdGridSearchNode>>();
        }

        public void setAstarOpenList(SortedList<int, INode> openList)
        {
            if (aStarOpenList != null)
            {
                throw new ApplicationException("setAstarOpenList must be called only once");
            }
            aStarOpenList = openList;
        }

        public bool ShouldPrune(INode node)
        {
            if (aStarOpenList == null)
            {
                throw new ApplicationException("setAstarOpenList must be called once before this method");
            }
            var newGridNode = node as RsdGridSearchNode;
            var hash = GetLinearHeadLocation(newGridNode);
            if (!HistoryNodes.ContainsKey(hash))
            {
                HistoryNodes[hash] = new List<RsdGridSearchNode>();
                HistoryNodes[hash].Add(newGridNode);
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
            }
            return false;
        }

        private long GetLinearHeadLocation(RsdGridSearchNode node)
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
