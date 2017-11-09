using System.Collections.Generic;
using MaxSearchAlg;

namespace MaSib.Domain.SIB
{
    interface ISibNode : INode
    {
        List<int> GetSnakeSpreadFreeSpots();
    }
}
