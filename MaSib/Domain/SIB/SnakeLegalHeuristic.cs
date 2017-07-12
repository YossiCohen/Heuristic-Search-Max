﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaSib.Domain.SIB
{
    public class SnakeLegalHeuristic : ISnakeHeuristic
    {
        public int calc_h(Snake s)
        {
            World world = s.world;
            int dim = world.Dimentions;
            int h = world.MaxPlacesInDimention;
            //2^n - Count illegle 
            for (int i = 0; i < world.MaxPlacesInDimention; i++)
            {
                bool valid = true;
                foreach (var part in s.tail)
                {
                    if (world.HammingDistance(i, part) < world.SnakeSpread)
                    {
                        valid = false;
                        break;
                    }
                }
                if (!valid)
                {
                    h--;
                }
                
            }
            return h + world.SnakeSpread; //+SnakeSpread because X next Locations head will be part of the non leagle
        }
    }
}