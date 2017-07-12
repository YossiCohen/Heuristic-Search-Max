using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaSib.Algorithms;

namespace MaSib.Domain.SIB
{
    public class SnakeNoneHeuristic : ISnakeHeuristic
    {
        public int calc_h(Snake s)
        {
            return s.world.MaxPlacesInDimention;
        }
    }
}
