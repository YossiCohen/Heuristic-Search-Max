
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace Grid.Domain
{
    public class BiconnectedComponents
    {
        private World w;
        private HashSet<int> cutPoints;
        private List<Tuple<int, int>> blockCutTree;
        private int[] _discovery;
        private int[] _low;
        private int[] _parent;

        public BiconnectedComponents(World w)
        {
            int count = 0;
            int time = 1;

            _discovery = new int[w.LinearSize];
            _low = new int[w.LinearSize];
            _parent = new int[w.LinearSize];

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
                //TODO continue with recursion 

            }

            //if (!w.IsBlocked(current) && !gridNode.IsVisited(current))
        }
    }
}
