using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaSib.Algorithms;

namespace MaSib.Domain.SIB
{
    interface ISibNode : INode
    {
        List<int> GetSnakeSpreadFreeSpots();
    }
}
