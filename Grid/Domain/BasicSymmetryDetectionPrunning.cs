using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
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
                    if (!historyNode.HeadLocation.Equals(newGridNode.HeadLocation))
                    {
                        continue;
                    }
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
            //They must be same length - unnecessary check!
            //if (a.length != a.length)
            //{
            //    return false;
            //}

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
