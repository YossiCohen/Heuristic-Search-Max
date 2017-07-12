using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaSib.Domain.SIB
{
    public interface ISnakeHeuristic
    {
        int calc_h(Snake s);
    }
}
