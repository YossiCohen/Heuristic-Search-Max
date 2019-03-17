using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Common;
using MaxSearchAlg;

namespace Grid.Domain
{
    public class HashedBasicSymmetryDetectionPrunning : IPrunningMethod
    {
        private Dictionary<int, Dictionary<int, List<GridSearchNode>>> HistoryNodes;
        public HashedBasicSymmetryDetectionPrunning()
        {
            HistoryNodes = new Dictionary<int, Dictionary<int, List<GridSearchNode>>>();
        }

        public bool ShouldPrune(INode node)
        {
            var newGridNode = node as GridSearchNode;
#if DEBUG
            Log.WriteLineIf($"[ShouldPrune - new node] {newGridNode.GetBitsString()}", TraceLevel.Info);
#endif
            var head = GetLinearHeadLocation(newGridNode);
            var hash = GetHashValue(newGridNode);
            if (!HistoryNodes.ContainsKey(head))
            {
                HistoryNodes[head] = new Dictionary<int, List<GridSearchNode>>();
                HistoryNodes[head].Add(hash, new List<GridSearchNode>());
                HistoryNodes[head][hash].Add(newGridNode);
#if DEBUG
                Log.WriteLineIf("[ShouldPrune - No list for head] - creating new", TraceLevel.Info);
#endif
            }
            else
            {
                if (!HistoryNodes[head].ContainsKey(hash))
                {
                    HistoryNodes[head].Add(hash, new List<GridSearchNode>());
                    HistoryNodes[head][hash].Add(newGridNode);
#if DEBUG
                    Log.WriteLineIf("[ShouldPrune - No list for hash] - creating new", TraceLevel.Info);
#endif
                }
                else
                {
                    var relevantList = HistoryNodes[head][hash];
                    foreach (var historyNode in relevantList)
                    {
                        //Log.WriteLineIf($"[ShouldPrune] - checking history node{historyNode.GetBitsString()}", TraceLevel.Info);
                        if (EqualBitArray(historyNode.Visited, newGridNode.Visited))
                        {
#if DEBUG
                            Log.WriteLineIf("[ShouldPrune]- true", TraceLevel.Info);
#endif
                            return true;
                        }
                    }
                    relevantList.Add(newGridNode);
                }
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

        private int GetHashValue(GridSearchNode node)
        {
            return ComputeFnvHash(BitArrayToByteArray(node.Visited));
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

        public static byte[] BitArrayToByteArray(BitArray bits)
        {
            byte[] ret = new byte[(bits.Length - 1) / 8 + 1];
            bits.CopyTo(ret, 0);
            return ret;
        }

        public static int ComputeFnvHash(params byte[] data)
        {  //from: https://stackoverflow.com/a/468084/1726419
            unchecked
            {
                const int p = 16777619;
                int hash = (int)2166136261;

                for (int i = 0; i < data.Length; i++)
                    hash = (hash ^ data[i]) * p;

                hash += hash << 13;
                hash ^= hash >> 7;
                hash += hash << 3;
                hash ^= hash >> 17;
                hash += hash << 5;
                return hash;
            }
        }


        public string GetName()
        {
            return "C HBSD_Pr";
        }
    }
}
