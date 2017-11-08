using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaxSearchAlg;

namespace Grid.Domain
{
    public class GridSearchNode : INode
    {
        public Location HeadLocation
        {
            get;
        }

        public GridSearchNode Parent
        {
            get;
        }

        private readonly BitArray _visited;

        public GridSearchNode(World world, GridSearchNode parentNode, MoveDirection move)
        {
            Parent = parentNode;
            HeadLocation = parentNode.HeadLocation.GetMovedLocation(move);
            _visited = new BitArray(parentNode._visited)
            {
                [HeadLocation.Y * world.Width + HeadLocation.X] = true
            };
            this.f = parentNode.f + 1;
            this.g = world.CalculateHeuristic(this);
            //TODO: Assert if newHeadLocation is illegal w.r.t parent head
        }

        public GridSearchNode(World world)
        {
            Parent = null;
            HeadLocation = world.Start;
            _visited = new BitArray(world.LinearSize);
            _visited[HeadLocation.Y * world.Width + HeadLocation.X] = true;
            this.f = 0;
            this.g = world.CalculateHeuristic(this);
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
