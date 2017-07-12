using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaSib.Domain.SIB
{
    public interface IBoxHeuristic
    {
        int calc_h(IBox b);
    }
}
