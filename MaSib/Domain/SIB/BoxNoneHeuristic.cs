using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaSib.Algorithms;

namespace MaSib.Domain.SIB
{
    public class BoxNoneHeuristic : IBoxHeuristic
    {
        public int calc_h(IBox s)
        {
            return s.world.MaxPlacesInDimention;
        }
    }
}
