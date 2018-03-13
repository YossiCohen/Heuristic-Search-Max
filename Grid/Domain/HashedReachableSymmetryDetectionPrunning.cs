using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Common;
using MaxSearchAlg;

namespace Grid.Domain
{
    public class HashedReachableSymmetryDetectionPrunning : IPrunningMethod
    {
        private Dictionary<int, Dictionary<long, List<RsdGridSearchNode>>> HistoryNodes;
        private SortedList<int, INode> aStarOpenList;
        private int cubeSize;

        public HashedReachableSymmetryDetectionPrunning(int cubeSize)
        {
            this.cubeSize = cubeSize;
            HistoryNodes = new Dictionary<int, Dictionary<long,List<RsdGridSearchNode>>>();
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
            Log.WriteLineIf($"[ShouldPrune - new node] {newGridNode.GetBitsString()}", TraceLevel.Info);
            var linearHead = GetLinearHeadLocation(newGridNode);
            var visitedBitmap = GetVisitedGrainedMap(newGridNode);
            if (!HistoryNodes.ContainsKey(linearHead))
            {
                HistoryNodes[linearHead] = new Dictionary<long, List<RsdGridSearchNode>>();
                HistoryNodes[linearHead].Add(visitedBitmap,new List<RsdGridSearchNode>());
                HistoryNodes[linearHead][visitedBitmap].Add(newGridNode);
                Log.WriteLineIf("[ShouldPrune - No list] - creating new", TraceLevel.Info);
            }
            else
            {/*
                var relevantList = HistoryNodes[linearHead];
                for (int i = relevantList.Count - 1; i >= 0; i--)
                {
                    var historyNode = relevantList[i];
                    Log.WriteLineIf($"[ShouldPrune] - checking history node{historyNode.GetBitsString()}", TraceLevel.Info);
                    //historyNode.Visited >= newGridNode.visited   ---- Simple case
                    if (ContainsOrEqualBitArray(historyNode.Visited, newGridNode.Visited))
                    {
                        if (ContainsOrEqualBitArray(historyNode.Reachable, newGridNode.Reachable))
                        {
                            Log.WriteLineIf("[ShouldPrune]- true1", TraceLevel.Info);
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
                                Log.WriteLineIf("[ShouldPrune] - remove and add", TraceLevel.Info);
                                //In this special case we replace the old node with new, we return true for pruning in order to count the old node as pruned
                                relevantList.RemoveAt(i);
                                relevantList.Add(newGridNode);
                                ReplaceInOpenList(historyNode, newGridNode);
                            }
                            Log.WriteLineIf("[ShouldPrune]- true2", TraceLevel.Info);
                            return true;
                        }
                    }
                }

                relevantList.Add(newGridNode); */
            }
            Log.WriteLineIf("[ShouldPrune] - false", TraceLevel.Info);
            return false;
        }

        private void ReplaceInOpenList(RsdGridSearchNode oldNode, RsdGridSearchNode newNode)
        {
            var a = aStarOpenList.Values.IndexOf(oldNode);
            aStarOpenList.RemoveAt(a);
            aStarOpenList.Add(newNode);
        }

        private int GetLinearHeadLocation(RsdGridSearchNode node)
        {
            return node.HeadLocation.Y * node.World.Width + node.HeadLocation.X;
        }

        public long GetVisitedGrainedMap(RsdGridSearchNode node)
        {
            long retVal = 0;
            var grainedGridW = (int)Math.Ceiling((decimal)node.World.Width / cubeSize);
            var grainedGridH = (int)Math.Ceiling((decimal)node.World.Height / cubeSize);
            var grainedGridSize = grainedGridW * grainedGridH;
            BitArray grained = new BitArray(grainedGridSize);
            int grainedX, grainedY;
            for (int y = 0; y < node.World.Height; y++)
            {
                for (int x = 0; x < node.World.Width; x++)
                {
                    if (node.IsVisited(x,y))
                    {
                        grainedX = (int)Math.Floor((decimal)x / cubeSize);
                        grainedY = (int)Math.Floor((decimal)y / cubeSize);
                        grained.Set(grainedX + grainedY * grainedGridW, true);
                        retVal |= 1 << (grainedX + grainedY * grainedGridW);
                    }
                }
            }
            return retVal; 
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
    }
}
