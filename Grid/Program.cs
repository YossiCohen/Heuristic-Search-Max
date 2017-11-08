using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Grid.Domain;

namespace Grid
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                System.Console.WriteLine(@"Please Provide arguments to run:");
                System.Console.WriteLine(@"all args should be in the form of: [key]=[value] with space between them");
                System.Console.WriteLine(@"Arguments:");
                System.Console.WriteLine(@"----------");
                System.Console.WriteLine(@"problem:     problem filename");
                return;
            }

            Dictionary<string, string> splitedArgs = SplitArguments(args);
            if (splitedArgs.ContainsKey("memtest") && splitedArgs["memtest"].Equals("true"))
            {
                MemTest();
            }

            int n = Int32.Parse(splitedArgs["dim"]);
            int sk = Int32.Parse(splitedArgs["snakespread"]);
            int bk = 2;
            if (splitedArgs.ContainsKey("boxspread"))
            {
                bk = Int32.Parse(splitedArgs["boxspread"]);
            }
            else
            {
                Log.WriteLineIf("boxSpread not found, setting it to:2", TraceLevel.Warning);
            }
        }

        private static void MemTest()
        {
            //MEMBLAST
            System.Console.WriteLine(@"Filling memory to check 64bit pointers - kill me when you want");
            List<String> m = new List<String>();
            long c = 0;
            while (true)
            {
                m.Add(Guid.NewGuid().ToString());
                c++;
                if (c % 1000000 == 0)
                {
                    Console.WriteLine(c + "toalmem:" + GC.GetTotalMemory(false));
                }
            }
        }

        private static Dictionary<string, string> SplitArguments(string[] args)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach (var s in args)
            {
                var splitted = s.Split('=');
                if (splitted.Length != 2)
                {
                    Console.WriteLine("WAIT! I can't understand:" + s);
                    throw new ArgumentException("Bad Argument" + s);
                }
                result.Add(splitted[0].ToLower(), splitted[1].ToLower());
            }
            return result;
        }
    }
}
