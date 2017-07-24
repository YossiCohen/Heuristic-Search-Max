using System.Collections.Generic;
using System.Linq;

namespace MaSib.Domain.SIB
{
    public class SnakeReachableHeuristic : ISnakeHeuristic
    {
        public int calc_h(Snake s)
        {
            World world = s.world;
            int dim = world.Dimentions;
            int basicSearchNode;
            List<int> open = new List<int>();
            int reachable = 0;
            bool[] closed = new bool[world.MaxPlacesInDimention];
            open.Add(s.Head);

            while (open.Count > 0)
            {
                int current = open.First();
                closed[current] = true;
                open.Remove(current);
                for (int i = 0; i < dim; i++)
                {
                    basicSearchNode = Snake.FlipBitAt(current, i);
                    if (closed[basicSearchNode])
                    {
                        continue;
                    }
                    closed[basicSearchNode] = true;
                    bool valid = true;
                    for (int j=0; j<s.tail.Length-world.SnakeSpread; j++)
                    {
                        if (World.HammingDistance(basicSearchNode, s.tail[j]) < world.SnakeSpread)
                        {
                            valid = false;
                            break;
                        }
                    }
                    if (valid)
                    {
                        reachable++;
                        open.Add(basicSearchNode);
                    }
                }

            }
            return reachable;

        }

    }
}
