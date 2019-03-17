using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
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

        [DebuggerDisplay("{GetBitsString(),nq}")]
        public BitArray Reachable;

        //code is duplicated - but using general activator will influance performance
        //https://stackoverflow.com/questions/6069661/does-system-activator-createinstancet-have-performance-issues-big-enough-to-di
        protected override LinkedList<INode> GenerateInitialChildList()
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

        public new string GetBitsString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"(V)");
            for (int i = 0; i < Visited.Length; i++)
            {
                sb.Append(Visited[i] ? "1" : "0");
                if (i % World.Width == World.Width - 1)
                {
                    sb.Append("-");
                }
            }
            sb.Append($"(R)");
            for (int i = 0; i < Reachable.Length; i++)
            {
                sb.Append(Reachable[i] ? "1" : "0");
                if (i % World.Width == World.Width - 1)
                {
                    sb.Append("-");
                }
            }
            sb.Append($"(g_{g}|h_{h})");
            return sb.ToString();
        }

    }
}
