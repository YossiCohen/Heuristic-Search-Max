using System;
using System.Collections;
using System.Collections.Generic;
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

        public GridSearchNode(GridSearchNode parentNode, MoveDirection move)
        {
            World = parentNode.World;
            Parent = parentNode;
            HeadLocation = parentNode.HeadLocation.GetMovedLocation(move);
            _visited = new BitArray(parentNode._visited)
            {
                [HeadLocation.Y * World.Width + HeadLocation.X] = true
            };
            g = parentNode.g + 1;
            h = World.CalculateHeuristic(this);

            //TODO: Assert if newHeadLocation is illegal w.r.t parent head
        }

        private World World;

        public GridSearchNode(World world)
        {
            World = world;
            Parent = null;
            HeadLocation = world.Start;
            _visited = new BitArray(world.LinearSize);
            _visited[HeadLocation.Y * world.Width + HeadLocation.X] = true;
            g = 0;
            h = world.CalculateHeuristic(this);
        }

        public int f {
            get { return g + h; }
        }
        public int h { get; }
        public int g { get; }

        public LinkedList<INode> Children
        {
            get
            {
                var result = GenerateInitialChildList();
                return result;
            }
        }

        private LinkedList<INode> GenerateInitialChildList()
        {

            LinkedList<INode> result = new LinkedList<INode>();
            if (HeadLocation.Y > 0)
            {
                result.AddLast(new GridSearchNode(this, MoveDirection.Up));
            }
            if (HeadLocation.Y < World.Height-1)
            {
                result.AddLast(new GridSearchNode(this, MoveDirection.Down));
            }
            if (HeadLocation.X > 0)
            {
                result.AddLast(new GridSearchNode(this, MoveDirection.Left));
            }
            if (HeadLocation.X < World.Width-1)
            {
                result.AddLast(new GridSearchNode(this, MoveDirection.Right));
            }
            return result;
        }


        public bool IsVisited(Location loc)
        {
            return _visited[loc.Y * World.Width + loc.X];
        }

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
