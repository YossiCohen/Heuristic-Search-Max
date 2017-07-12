using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaSib.Algorithms
{
    public interface INode
    {
        int f { get; }

        int h { get; }

        int g { get; }

        LinkedList<INode> Children { get; }

        string GetBitsString();

        string GetIntString();
    }
}
