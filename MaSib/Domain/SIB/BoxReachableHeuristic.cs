using System;
using System.Collections.Generic;
using System.Linq;

namespace MaSib.Domain.SIB
{
    public class BoxReachableHeuristic : IBoxHeuristic
    {
        public int calc_h(Box b)
        {
            World world = b.world;
            int dim = world.Dimentions;
            int basicSearchNode;

            List<int> open = new List<int>();
            int reachable = 0;
            bool[] closed = new bool[world.MaxPlacesInDimention];

            int minimalSpread = Math.Min(world.SnakeSpread, world.BoxSpread);
            foreach (var bSnake in b.snakes)
            {
                open.Add(bSnake.Head);
            }

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
                    foreach (var bSnake in b.snakes)
                    {
                        for (int j = 0; j < bSnake.tail.Length - minimalSpread; j++)
                        {
                            if (World.HammingDistance(basicSearchNode, bSnake.tail[j]) < minimalSpread)
                            {
                                valid = false;
                                break;
                            }
                        }
                        if (!valid)
                        {
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
