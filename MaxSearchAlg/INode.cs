using System.Collections.Generic;

namespace MaxSearchAlg
{
    public interface INode
    {
        int f { get; }

        int h { get; }

        int g { get; }

        LinkedList<INode> Children { get; }

        string GetBitsString();

        string GetNodeStringV2();
    }
}
