using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

namespace Grid.Domain
{
    public class BiconnectedComponents
    {
        private World World;
        public LinkedList<HashSet<int>> Blocks { get; }
        public HashSet<int> CutPoints { get; }
        private int[] _discovery;
        private int[] _low;
        private int[] _parent;
        private Stack<Edge> _edgeStack;
        private int time;
        private Dictionary<int, List<int>> _blockCutTree_CutPointsToBlockId;
        private Dictionary<int, List<int>> _blockCutTree_BlockIdToCutPoints;
        private BitArray blockedOrVisited;

        public BiconnectedComponents(World world, GridSearchNode searchNode = null)
        {
            time = 1;
            World = world;
            _discovery = new int[World.LinearSize];
            _low = new int[World.LinearSize];
            _parent = new int[World.LinearSize];
            _edgeStack = new Stack<Edge>();
            Blocks = new LinkedList<HashSet<int>>();
            CutPoints = new HashSet<int>();
            _blockCutTree_CutPointsToBlockId = new Dictionary<int, List<int>>();
            _blockCutTree_BlockIdToCutPoints = new Dictionary<int, List<int>>();
            if (null == searchNode)
            {
                blockedOrVisited = World._isBlockedLocations;
            }
            else
            {
                blockedOrVisited = World.GetBlockedOrVisited(searchNode);
                //TODO: need to check deeply - deal the head location as non visited place so the BCC can add it to blocks
                blockedOrVisited[searchNode.HeadLocation.X + (searchNode.HeadLocation.Y * World.Width)] = false;
            }

            // Initialize arrays
            for (int i = 0; i < World.LinearSize; i++)
            {
                _discovery[i] = -1;
                _low[i] = -1;
                _parent[i] = -1;
            }

            for (int i = 0; i < World.LinearSize; i++)
            {
                if (IsBlockedLinear(i)) continue;
                if (_discovery[i] == -1)
                {
                    RecursiveBcc(i);
                }
                // If stack is not empty, pop all edges from stack
                if (_edgeStack.Count > 0)
                {
                    HashSet<int> blk = new HashSet<int>();
                    while (_edgeStack.Count > 0)
                    {

                        Edge edgeInBlock = _edgeStack.Pop();
                        blk.Add(edgeInBlock.U);
                        blk.Add(edgeInBlock.V);
                    }
                    Blocks.AddLast(blk);
                }
            }
            InitCutpoints();
            InitBlockCutTree();
        }

        private void RecursiveBcc(int u)
        {
            // Initialize discovery time and low value
            _discovery[u] = _low[u] = time++;

            //near nodes to check
            List<int> childrens = new List<int>();
            if ((u + 1) % World.Width != 0 && !IsBlockedLinear(u + 1)) childrens.Add(u + 1); //right location
            if (u % World.Width != 0 && !IsBlockedLinear(u - 1)) childrens.Add(u - 1); //left location
            if (!IsBlockedLinear(u + World.Width)) childrens.Add(u + World.Width); //down location
            if (!IsBlockedLinear(u - World.Width)) childrens.Add(u - World.Width); //up location

            foreach (var v in childrens)
            {
                if (_discovery[v] == -1) //child not visited, continue recursion
                {
                    _parent[v] = u;
                    _edgeStack.Push(new Edge(u, v));
                    RecursiveBcc(v);

                    // Case 1 (Strongly Connected Components Article)
                    // Check if the subtree rooted with 'v' has a connection to one of the ancestors of 'u'
                    if (_low[u] > _low[v])
                    {
                        _low[u] = _low[v];
                    }
                    // If u is an articulation point,
                    // pop all edges from stack till u -- v
                    if ((_discovery[u] == 1 && childrens.Count > 1) || (_discovery[u] > 1 && _low[v] >= _discovery[u]))
                    {
                        Edge edgeInBlock;
                        HashSet<int> blk = new HashSet<int>();
                        while (_edgeStack.Peek().U != u || _edgeStack.Peek().V != v)
                        {
                            edgeInBlock = _edgeStack.Pop();
                            blk.Add(edgeInBlock.U);
                            blk.Add(edgeInBlock.V);
                        }
                        edgeInBlock = _edgeStack.Pop();
                        blk.Add(edgeInBlock.U);
                        blk.Add(edgeInBlock.V);
                        Blocks.AddLast(blk);
                    }
                }

                // Case 2 (Strongly Connected Components Article)
                // Update low value of 'u' only of 'v' is still in stack (i.e. it's a back edge, not cross edge).
                else if (v != _parent[u] && _discovery[v] < _low[u])
                {
                    if (_low[u] > _discovery[v])
                    {
                        _low[u] = _discovery[v];
                    }
                    _edgeStack.Push(new Edge(u, v));
                }
            } //foreach childrens
        }

        private bool IsBlockedLinear(int loc)
        {
            if (loc < 0 || loc >= blockedOrVisited.Count) return true;
            return blockedOrVisited[loc];
        }

        private void InitCutpoints()
        {
            int[] tmpCounter = new int[World.LinearSize];
            foreach (var blk in Blocks)
            {
                foreach (var linearLoc in blk)
                {
                    tmpCounter[linearLoc]++;
                }
            }
            for (int i = 0; i < tmpCounter.Length; i++)
            {
                if (tmpCounter[i] > 1)
                {
                    CutPoints.Add(i);
                }
            }
        }

        private void InitBlockCutTree()
        {
            for (int i = 0; i < Blocks.Count; i++)
            {
                var blk = Blocks.ElementAt(i);
                foreach (var item in blk)
                {
                    if (CutPoints.Contains(item)) //contains(item))
                    {
                        if (!_blockCutTree_BlockIdToCutPoints.ContainsKey(i))
                        {
                            _blockCutTree_BlockIdToCutPoints.Add(i, new List<int>());
                        }
                        _blockCutTree_BlockIdToCutPoints[i].Add(item);
                        if (!_blockCutTree_CutPointsToBlockId.ContainsKey(item))
                        {
                            _blockCutTree_CutPointsToBlockId.Add(item, new List<int>());
                        }
                        _blockCutTree_CutPointsToBlockId[item].Add(i);
                    }
                }
            }
        }

        public bool[] GetValidPlacesForMaxPath(Location startPoint, Location destinationPoint)
        {
            return GetValidPlacesForMaxPath(startPoint.GetLinearLocationRepresentation(World),
                destinationPoint.GetLinearLocationRepresentation(World));
        }

        public bool[] GetValidPlacesForMaxPath(int startPoint, int destinationPoint)
        {
            List<int> relevantBlocks = GetRelevantBlocks(startPoint, destinationPoint);
            bool[] validPlaces = new bool[World.LinearSize];
            if (relevantBlocks == null)
            {
                return validPlaces;
            }
            foreach (var relevantBlock in relevantBlocks)
            {
                var itemSet = Blocks.ElementAt(relevantBlock);
                foreach (var linearLocation in itemSet)
                {
                    validPlaces[linearLocation] = true;
                }
            }
            return validPlaces;
        }

        public bool LinearLocationWasVisitedDuringBuild(Location position)
        {
            return _parent[position.GetLinearLocationRepresentation(World)] != -1;
        }

        private List<int> GetRelevantBlocks(int startPoint, int destinationPoint)
        {
            bool[] seenBlocks = new bool[Blocks.Count];
            var startBlocks = GetBlockIdsOfLinearLocation(startPoint);
            var destinationBlocks = GetBlockIdsOfLinearLocation(destinationPoint);
            return GetRelevantBlocksRecursive(startBlocks, destinationBlocks, seenBlocks);
        }

        private List<int> GetRelevantBlocksRecursive(List<int> startPointBlocks, List<int> destinationPointBlock, bool[] seenBlocks)
        {
            //endCondition
            foreach (var startBlock in startPointBlocks)
            {
                foreach (var destBlock in destinationPointBlock)
                {
                    if (startBlock == destBlock)
                    {
                        return new List<int>(){startBlock};
                    }
                }
                seenBlocks[startBlock] = true;
            }

            foreach (var startBlock in startPointBlocks)
            {
                if (!_blockCutTree_BlockIdToCutPoints.ContainsKey(startBlock)) return null; //Block cut tree is disconnected
                foreach (var cutPoint in _blockCutTree_BlockIdToCutPoints[startBlock])
                {
                    var searchBlocks = _blockCutTree_CutPointsToBlockId[cutPoint];
                    for (int i = 0; i < searchBlocks.Count; i++)
                    {
                        if (seenBlocks[searchBlocks[i]])
                        {
                            searchBlocks.RemoveAt(i);
                            i--;
                        }
                    }
                    if (searchBlocks.Count == 0) continue;

                    var aa = GetRelevantBlocksRecursive(_blockCutTree_CutPointsToBlockId[cutPoint].ToList(),
                        destinationPointBlock, seenBlocks);
                    if (aa != null)
                    {
                        aa.Add(startBlock);
                        return aa;
                    }
                }
            }
            return null;
        }

        private List<int> GetBlockIdsOfLinearLocation(int linearLoc)
        {
            var retVal = new List<int>();
            for (int i = 0; i < Blocks.Count; i++)
            {
                var blk = Blocks.ElementAt(i);
                if (blk.Contains(linearLoc))
                {
                    retVal.Add(i);
                }
            }
            return retVal;
        }

        
    }

    public class Edge
    {
        public int U;
        public int V;
        public Edge(int u, int v)
        {
            U = u;
            V = v;
        }
    };
}
