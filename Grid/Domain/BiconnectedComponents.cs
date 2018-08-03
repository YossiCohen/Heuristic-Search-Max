using System;
using System.Collections.Generic;
using System.Linq;

namespace Grid.Domain
{
    public class BiconnectedComponents
    {
        private World w;
        private LinkedList<HashSet<int>> _blocks;
        private HashSet<int> _cutPoints;
        private List<Tuple<int, int>> _blockCutTree;
        private int[] _discovery;
        private int[] _low;
        private int[] _parent;
        private Stack<Edge> _edgeStack;
        private int time;

        public BiconnectedComponents(World _w)
        {
            w = _w;
            _discovery = new int[w.LinearSize];
            _low = new int[w.LinearSize];
            _parent = new int[w.LinearSize];
            _edgeStack = new Stack<Edge>();
            _blocks = new LinkedList<HashSet<int>>();
            _cutPoints = new HashSet<int>();
            _blockCutTree = new List<Tuple<int, int>>();

            // Initialize arrays
            for (int i = 0; i < w.LinearSize; i++)
            {
                _discovery[i] = -1;
                _low[i] = -1;
                _parent[i] = -1;
            }

            for (int i = 0; i < w.LinearSize; i++)
            {
                if(w.IsBlockedLinear(i)) continue;
                if (_discovery[i] == -1)
                {
                    RecursiveBCC(i);
                }
                // If stack is not empty, pop all edges from stack
                if (_edgeStack.Count > 0)
                {
                    HashSet<int> blk = new HashSet<int>();
                    while (_edgeStack.Count > 0)
                    {

                        Edge edgeInBlock = _edgeStack.Pop();
                        blk.Add(edgeInBlock.u);
                        blk.Add(edgeInBlock.v);
                    }
                    _blocks.AddLast(blk);
                }
            }
            findCutpoints();
            findBlockCutTree();
        }

        private void RecursiveBCC(int u)
        {
            // Initialize discovery time and low value
            _discovery[u] = _low[u] = time++;

            //near nodes to check
            List<int> childrens = new List<int>();
            if ((u+1)%w.Width != 0 && !w.IsBlockedLinear(u + 1)) childrens.Add(u + 1); //right location
            if (u % w.Width != 0 && !w.IsBlockedLinear(u - 1)) childrens.Add(u - 1); //left location
            if (!w.IsBlockedLinear(u + w.Width)) childrens.Add(u + w.Width); //down location
            if (!w.IsBlockedLinear(u - w.Width)) childrens.Add(u - w.Width); //up location

            foreach (var v in childrens)
            {
                if (_discovery[v] == -1) //child not visited, continue recursion
                {
                    _parent[v] = u;
                    _edgeStack.Push(new Edge(u, v));
                    RecursiveBCC(v);

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
                        while (_edgeStack.Peek().u != u || _edgeStack.Peek().v != v)
                        {
                            edgeInBlock = _edgeStack.Pop();
                            blk.Add(edgeInBlock.u);
                            blk.Add(edgeInBlock.v);
                        }
                        edgeInBlock = _edgeStack.Pop();
                        blk.Add(edgeInBlock.u);
                        blk.Add(edgeInBlock.v);
                        _blocks.AddLast(blk);
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
        
        private void findCutpoints()
        {
            int[] tmpCounter = new int[w.LinearSize];
            foreach (var blk in _blocks)
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
                    _cutPoints.Add(i);
                }
            }
        }

        private void findBlockCutTree()
        {
            for (int i = 0; i < _blocks.Count; i++)
            {
                var blk = _blocks.ElementAt(i);
                foreach (var item in blk)
                {
                    if (_cutPoints.Contains(item))   //contains(item))
                    {
                        _blockCutTree.Add(new Tuple<int, int>(-item, i));
                        _blockCutTree.Add(new Tuple<int, int>(i, -item));
                    }
                }
            }
        }
    }

    public class Edge
    {
        public int u;
        public int v;
        public Edge(int u, int v)
        {
            this.u = u;
            this.v = v;
        }
    };
}
