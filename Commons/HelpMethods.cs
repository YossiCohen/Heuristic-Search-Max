using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons
{
    public class HelpMethods
    {
        public static Int32 GetCardinality(BitArray bitArray)
        {   //As explained in: http://graphics.stanford.edu/~seander/bithacks.html#CountBitsSetParallel
            //and implemented at: https://stackoverflow.com/a/14354311/1726419
            Int32[] ints = new Int32[(bitArray.Count >> 5) + 1];

            bitArray.CopyTo(ints, 0);

            Int32 count = 0;

            // fix for not truncated bits in last integer that may have been set to true with SetAll()
            ints[ints.Length - 1] &= ~(-1 << (bitArray.Count % 32));

            for (Int32 i = 0; i < ints.Length; i++)
            {

                Int32 c = ints[i];

                // magic (http://graphics.stanford.edu/~seander/bithacks.html#CountBitsSetParallel)
                unchecked
                {
                    c = c - ((c >> 1) & 0x55555555);
                    c = (c & 0x33333333) + ((c >> 2) & 0x33333333);
                    c = ((c + (c >> 4) & 0xF0F0F0F) * 0x1010101) >> 24;
                }

                count += c;

            }

            return count;

        }
    }
}
