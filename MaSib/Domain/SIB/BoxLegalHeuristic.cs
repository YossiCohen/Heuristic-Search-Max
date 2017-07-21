
using System;

namespace MaSib.Domain.SIB
{
    public class BoxLegalHeuristic : IBoxHeuristic
    {
        public int calc_h(Box b)
        {
            World world = b.world;
            int dim = world.Dimentions;
            int h = world.MaxPlacesInDimention;
            int minimalSpread = Math.Min(world.SnakeSpread, world.BoxSpread);
            //2^n - Count illegle 
            for (int i = 0; i < world.MaxPlacesInDimention; i++)
            {
                bool valid = true;
                foreach (var bSnake in b.snakes)
                {
                    foreach (var part in bSnake.tail)
                    {
                        if (World.HammingDistance(i, part) < minimalSpread)
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

                if (!valid)
                {
                    h--;
                }
                
            }
            return h + minimalSpread; //+SnakeSpread because X next Locations head will be part of the non leagle
        }
    }
}
