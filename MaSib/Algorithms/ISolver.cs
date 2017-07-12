using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaSib.Algorithms
{
    public interface ISolver
    {
        int Expended { get;}

        int Generated { get;}

        int Pruned { get; }

        State Run();

        INode GetMaxGoal();
    }
}
