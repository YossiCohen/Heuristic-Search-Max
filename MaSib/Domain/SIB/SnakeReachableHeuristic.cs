using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaSib.Domain.SIB
{
    public class SnakeReachableHeuristic : ISnakeHeuristic
    {
        public int calc_h(Snake s)
        {
            World world = s.world;
            int dim = world.Dimentions;
            int dfsNode;
            HashSet<int> open = new HashSet<int>();
            HashSet<int> Reachable = new HashSet<int>();
            HashSet<int> closed = new HashSet<int>();
            open.Add(s.Head);

            while (open.Count > 0)
            {
                int current = open.First();
                closed.Add(current);
                open.Remove(current);
                for (byte i = 0; i < dim; i++)
                {
                    dfsNode = Snake.FlipBitAt(current, i);
                    if (closed.Contains(dfsNode))
                    {
                        continue;
                    }
                    closed.Add(dfsNode);
                    bool valid = true;
//                    foreach (var part in s.tail)
                    for (byte j=0; j<s.tail.Length-world.SnakeSpread; j++)
                    {
                        if (World.HammingDistance(dfsNode, s.tail[j]) < world.SnakeSpread)
                        {
                            valid = false;
                            break;
                        }
                    }
                    if (valid)
                    {
                        Reachable.Add(dfsNode);
                        open.Add(dfsNode);
                    }
                }

            }
            return Reachable.Count;

        }

    }
}
