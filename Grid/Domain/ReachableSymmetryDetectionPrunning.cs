using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Common;
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
#if DEBUG
            if (aStarOpenList == null)
            {
                throw new ApplicationException("setAstarOpenList must be called once before this method");
            }
#endif
            var newGridNode = node as RsdGridSearchNode;
#if DEBUG
            Log.WriteLineIf($"[ShouldPrune - new node] {newGridNode.GetBitsString()}", TraceLevel.Info);
#endif
            var hash = GetLinearHeadLocation(newGridNode);
            if (!HistoryNodes.ContainsKey(hash))
            {
                HistoryNodes[hash] = new List<RsdGridSearchNode>();
                HistoryNodes[hash].Add(newGridNode);
#if DEBUG
                Log.WriteLineIf("[ShouldPrune - No list] - creating new", TraceLevel.Info);
#endif
            }
            else
            {
                var relevantList = HistoryNodes[hash];
                for (int i = relevantList.Count - 1; i >= 0; i--)
                {
                    var historyNode = relevantList[i];
#if DEBUG
                    Log.WriteLineIf($"[ShouldPrune] - checking history node{historyNode.GetBitsString()}", TraceLevel.Info);
#endif
                    //historyNode.Visited >= newGridNode.visited   ---- Simple case
                    if (ContainsOrEqualBitArray(historyNode.Visited, newGridNode.Visited))
                    {
                        if (ContainsOrEqualBitArray(historyNode.Reachable, newGridNode.Reachable))
                        {
#if DEBUG
                            Log.WriteLineIf("[ShouldPrune]- true1", TraceLevel.Info);
#endif
                            return true;
                        }
                    }
                    //historyNode.Visited < newGridNode.visited
                    else if (ContainsOrEqualBitArray(newGridNode.Visited, historyNode.Visited))
                    {
                        if (ContainsOrEqualBitArray(newGridNode.Reachable, historyNode.Reachable)) 
                        {
                            if (newGridNode.g > historyNode.g)
                            {
#if DEBUG
                                Log.WriteLineIf("[ShouldPrune] - remove and add", TraceLevel.Info);
#endif
                                //In this special case we replace the old node with new, we return true for pruning in order to count the old node as pruned
                                relevantList.RemoveAt(i);
                                relevantList.Add(newGridNode);
                                ReplaceInOpenList(historyNode, newGridNode);
                            }
#if DEBUG
                            Log.WriteLineIf("[ShouldPrune]- true2", TraceLevel.Info);
#endif
                            return true;
                        }
                    }
                }

                relevantList.Add(newGridNode);
            }
#if DEBUG
            Log.WriteLineIf("[ShouldPrune] - false", TraceLevel.Info);
#endif
            return false;
        }

        private void ReplaceInOpenList(RsdGridSearchNode oldNode, RsdGridSearchNode newNode)
        {
            var a = aStarOpenList.Values.IndexOf(oldNode);
            aStarOpenList.RemoveAt(a);
            aStarOpenList.Add(newNode);
        }

        private long GetLinearHeadLocation(RsdGridSearchNode node)
        {
            return node.HeadLocation.Y * node.World.Width + node.HeadLocation.X;
        }

        private bool ContainsOrEqualBitArray(BitArray larger, BitArray smaller)
        {
            BitArray tmp = (BitArray)larger.Clone();
            tmp.Or(smaller);
            tmp.Xor(larger);
            foreach (bool b in tmp)
            {
                if (b) return false;
            }
            return true;
        }

        public string GetName()
        {
            return "D RSD_Pr";
        }
    }
}
