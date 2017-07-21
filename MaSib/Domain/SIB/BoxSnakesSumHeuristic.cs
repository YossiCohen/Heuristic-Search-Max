using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaSib.Domain.SIB
{
    public class BoxSnakesSumHeuristic : IBoxHeuristic
    {
        public int calc_h(Box b)
        {
            var h = 0;
            var totalG = 0;
            foreach (var bSnake in b.snakes)
            {
                h += bSnake.h;
                totalG += bSnake.g;
            }
            return Math.Min(h, b.world.MaxPlacesInDimention-totalG);
        }
    }
}
