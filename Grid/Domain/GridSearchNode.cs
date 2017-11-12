using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
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
        [DebuggerDisplay("{GetBitsString(),nq}")]
        protected internal readonly BitArray Visited;

        public GridSearchNode(GridSearchNode parentNode, MoveDirection move)
        {
            World = parentNode.World;
            Parent = parentNode;
            HeadLocation = parentNode.HeadLocation.GetMovedLocation(move);
            Visited = new BitArray(parentNode.Visited)
            {
                [HeadLocation.Y * World.Width + HeadLocation.X] = true
            };
            g = parentNode.g + 1;
            h = World.CalculateHeuristic(this);
        }

        internal World World;

        public GridSearchNode(World world)
        {
            World = world;
            Parent = null;
            HeadLocation = world.Start;
            Visited = new BitArray(world.LinearSize);
            Visited[HeadLocation.Y * world.Width + HeadLocation.X] = true;
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
                //Hack - If Goal node - it will have no children
                if (World.Goal.Equals(HeadLocation))
                {
                    return new LinkedList<INode>();
                }
                var result = GenerateInitialChildList();
                result = RemoveChildrensOnVisitedAndBlockedLocations(result);
                return result;
            }
        }

        private LinkedList<INode> RemoveChildrensOnVisitedAndBlockedLocations(LinkedList<INode> childs)
        {
            LinkedList<INode> result = new LinkedList<INode>();
            foreach (var node in childs)
            {
                var child = (GridSearchNode) node;
                if (!(Visited[child.HeadLocation.Y * World.Width + child.HeadLocation.X] || (World.IsBlocked(child.HeadLocation))))
                {
                    result.AddLast(child);
                }
            }
            return result;
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
            return Visited[loc.Y * World.Width + loc.X];
        }

        public string GetBitsString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Visited.Length; i++)
            {
                sb.Append(Visited[i] ? "1" : "0");
                if (i % World.Width == World.Width - 1)
                {
                    sb.Append("-");
                }
            }
            sb.Append($"(g_{g}|h_{h})");
            return sb.ToString();
        }

        public override string ToString()
        {
            return String.Format("G:{0} Bits:{1}", g, GetBitsString());
        }

        public string GetNodeStringV2()
        {
            StringBuilder sb = new StringBuilder();
            GridSearchNode cursor = this;
            while (cursor != null)
            {
                sb.Append(cursor.HeadLocation);
                sb.Append("_");
                cursor = cursor.Parent;
            }
            return sb.ToString();
        }
    }
}
