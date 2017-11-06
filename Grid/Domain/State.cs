using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaxSearchAlg;

namespace Grid.Domain
{
    public class State : INode
    {
        public Location HeadLocation
        {
            get;
        }

        private readonly BitArray _visited;

        public State(World world, Location loc)
        {
            HeadLocation = loc;
            _visited = new BitArray(world.LinearSize);
            _visited[loc.Y * world.Width + loc.X] = true;
        }

        public int f { get; }
        public int h { get; }
        public int g { get; }
        public LinkedList<INode> Children { get; }
        public string GetBitsString()
        {
            throw new NotImplementedException();
        }

        public string GetIntString()
        {
            throw new NotImplementedException();
        }
    }
}
