using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grid.Domain
{
    public interface IGridHeuristic
    {
        int calc_h(World w, GridSearchNode gridNode);
    }
}
