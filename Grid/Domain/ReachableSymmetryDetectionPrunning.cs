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
            //The algorithms add current node if this method returns false.
            //in case this node is needs pruning because it is "smaller" then existin we return TRUE
            //when the node in OPEN is smaller we return FALSE and this method will REPLACE the nodes
            //That way the alg. will count the prunning nodes correctly
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
                    //historyNode.Visited >= newGridNode.visited
                    if (ContainsOrEqualBitArray(historyNode.Visited, newGridNode.Visited))
                    {
                        if (ContainsOrEqualBitArray(historyNode.Reachable, newGridNode.Reachable))
                        {
                            return true;
                        }
                    }
                    //historyNode.Visited < newGridNode.visited
                    else 
                    {
                        //TODO: continue
                        //if (ContainsOrEqualBitArray(newGridNode.Reachable, historyNode.Reachable))
                        //{
                        //    return true;
                        //}
                    }



                    //TODO:REMOVE - this is from BSD
                    //if (ContainsOrEqualBitArray(historyNode.Visited,newGridNode.Visited))
                    //{
                    //    return true;
                    //}

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

        private bool ContainsOrEqualBitArray(BitArray larger, BitArray smaller)
        {
            int finalIndex = larger.Length - 1;
            for (int i = 0; i < finalIndex; i++)
            {
                //TODO: fix the condition
                if (larger[i] & !smaller[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
