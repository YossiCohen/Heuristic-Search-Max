using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaSib.Domain.SIB
{
    public interface IBox
    {
        World world { get; }
        Snake[] snakes { get; }
    }
}
