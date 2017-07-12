using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaSib
{
    public class World
    {
        public const int MAX_DIM = 13;
        private readonly int dimentions;
        private readonly int snakeSpread;
        private readonly int boxSpread;
        private readonly int maxValForDimentions;


        public World(int dimentions, int snakeSpread, int boxSpread = 100)
        {
            if (dimentions > MAX_DIM)
            {
                throw new ArgumentException("Too many dimentions - I'm confused");
            }
            this.dimentions = dimentions;
            this.snakeSpread = snakeSpread;
            this.boxSpread = boxSpread;
            this.maxValForDimentions = (int)Math.Pow(2, dimentions);
        }

        public int Dimentions
        {
            get { return dimentions;}
        }

        public int MaxPlacesInDimention
        {
            get { return maxValForDimentions; }
        }

        public int SnakeSpread
        {
            get { return snakeSpread; }
        }

        public int BoxSpread
        {
            get { return boxSpread; }
        }

        public bool ValidPosition(int pos)
        {
            return (pos < maxValForDimentions); 
        }

        public string IntToBitString(int input)
        {
            if (!ValidPosition(input))
            {
                throw new ArgumentException("Input not from this world");
            }
            char[] bits = new char[this.dimentions];
            int i = 0;

            while (i< this.dimentions)
            {
                bits[i++] = (input & 1) == 1 ? '1' : '0';
                input >>= 1;
            }
            Array.Reverse(bits, 0, i);
            char[] tmp = new char[i];
            Array.Copy(bits, 0, tmp, 0, i);
            return new string(tmp);
        }

        public string IntArrToBitString(int[] input)
        {
            StringBuilder sb = new StringBuilder();
            string prefix = "";
            foreach (var i in input)
            {
                sb.Append(prefix);
                prefix = "-";
                sb.Append(IntToBitString(i));
            }
            return sb.ToString();
        }

        public int HammingDistance(int a, int b)
        {
            int val = a ^ b;
            int dist = 0;
            while (val != 0)
            {
                val = val & (val - 1);
                dist++;
            }
            return dist;
        }

        public override string ToString()
        {
            return String.Format("World::N:{0},sK:{1},bK:{2}", dimentions, snakeSpread, boxSpread);
        }
    }
}
