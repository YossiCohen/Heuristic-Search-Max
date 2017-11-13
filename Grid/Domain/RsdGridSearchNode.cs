using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaxSearchAlg;

namespace Grid.Domain
{
    public class RsdGridSearchNode : GridSearchNode
    {
        public RsdGridSearchNode(GridSearchNode parentNode, MoveDirection move) : base(parentNode, move)
        {
        }

        public RsdGridSearchNode(World world) : base(world)
        {
        }

        [DebuggerDisplay("{GetBitsString(),nq}")] public BitArray Reachable;

        //TODO code is duplicated - refactor needed
        public new LinkedList<INode> Children
        {
            get
            {
                //If Goal node - it will have no children
                if (World.Goal.Equals(HeadLocation))
                {
                    return new LinkedList<INode>();
                }
                var result = GenerateInitialChildList();
                result = RemoveChildrensOnVisitedAndBlockedLocations(result);
                return result;
            }
        }

        //TODO code is duplicated - refactor needed
        private LinkedList<INode> RemoveChildrensOnVisitedAndBlockedLocations(LinkedList<INode> childs)
        {
            LinkedList<INode> result = new LinkedList<INode>();
            foreach (var node in childs)
            {
                var child = (RsdGridSearchNode)node;
                if (!(Visited[child.HeadLocation.Y * World.Width + child.HeadLocation.X] || (World.IsBlocked(child.HeadLocation))))
                {
                    result.AddLast(child);
                }
            }
            return result;
        }

        //TODO code is duplicated - refactor needed
        private LinkedList<INode> GenerateInitialChildList()
        {

            LinkedList<INode> result = new LinkedList<INode>();
            if (HeadLocation.Y > 0)
            {
                result.AddLast(new RsdGridSearchNode(this, MoveDirection.Up));
            }
            if (HeadLocation.Y < World.Height - 1)
            {
                result.AddLast(new RsdGridSearchNode(this, MoveDirection.Down));
            }
            if (HeadLocation.X > 0)
            {
                result.AddLast(new RsdGridSearchNode(this, MoveDirection.Left));
            }
            if (HeadLocation.X < World.Width - 1)
            {
                result.AddLast(new RsdGridSearchNode(this, MoveDirection.Right));
            }
            return result;
        }

    }
}
