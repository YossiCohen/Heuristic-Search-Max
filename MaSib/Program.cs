using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using Common;
using MaSib.Algorithms;
using MaSib.Domain.SIB;

namespace MaSib
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
                System.Console.WriteLine(@"problem:     [snake/box/box-od] snake is single agent & box is multi-agent");
                System.Console.WriteLine(@"             if you choose box you must provide snakes initial locations");
                System.Console.WriteLine(@"             with arguments Sx=location");
                System.Console.WriteLine(@"Sx:          starting location of snake number x, counting from 0");
                System.Console.WriteLine(@"             when using snake you can have only 1 Sx argument");
                System.Console.WriteLine(@"snakeH:      [none/legal/reachable] the snake heuristic");
                System.Console.WriteLine(@"boxH:        [none/snakes-sum] the box heuristic");
                System.Console.WriteLine(@"alg:         [astar/dfbnb] the solving algorithm");
                System.Console.WriteLine(@"dim:         the number of dimentions for the problem (N)");
                System.Console.WriteLine(@"snakeSpread: the intra-snake spread (sK)");
                System.Console.WriteLine(@"boxSpread:   the inter-snake spread (bK)");
                System.Console.WriteLine(@"timeLimit:   limit run time to X minutes (default 120), 0 for no time limit");
                System.Console.WriteLine(@"memTest:     if set to true, will not solve nothing, only fill memory");
                System.Console.WriteLine(@"             allocation to check 64bit issue");
                System.Console.WriteLine(@"");
                System.Console.WriteLine(@"Start examples:");
                System.Console.WriteLine(@"---------------");
                System.Console.WriteLine(@"this is how to solve single snake problem with A* (head at 0=(00000))");
                System.Console.WriteLine(@"when the dimention is set to 5 and snake spread is 2:");
                System.Console.WriteLine(@"MaSiB problem=snake S0=0 alg=astar dim=5 snakeSpread=2");
                System.Console.WriteLine(@"");
                System.Console.WriteLine(@"this is how to solve multiple snake problem with A*-OD");
                System.Console.WriteLine(@"when the dimention is set to 7, intra-snake spread is 2");
                System.Console.WriteLine(@"and inter-snake spread is 3, the starting locations are 0-(0000000)");
                System.Console.WriteLine(@"and 127-(1111111) so we have 2 snakes");
                System.Console.WriteLine(@"MaSiB problem=box-od s0=0 s1=127 alg=dfbnb dim=7 snakeSpread=2 boxSpread=3 boxh=snakes-sum snakeh=reachable");
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
            
            World w = new World(n, sk, bk);
            ISibNode initState;
            ISnakeHeuristic snakeh;
            IBoxHeuristic boxh;
            Solver solver;
            if (!splitedArgs.ContainsKey("boxh")) //default boxh
            {
                splitedArgs.Add("boxh","none");
            }
            switch (splitedArgs["boxh"])
            {
                case "none":
                    boxh = new BoxNoneHeuristic();
                    break;
                case "snakes-sum":
                    boxh = new BoxSnakesSumHeuristic();
                    break;
                default:
                    Log.WriteLineIf("Box heuristic: "+ splitedArgs["boxh"] + " is not supported!", TraceLevel.Error);
                    return;
                    break;
            }

            if (!splitedArgs.ContainsKey("snakeh")) //default snakeh
            {
                splitedArgs.Add("snakeh", "none");
            }
            switch (splitedArgs["snakeh"])
            {
                case "none":
                    snakeh = new SnakeNoneHeuristic();
                    break;
                case "legal":
                    snakeh = new SnakeLegalHeuristic();
                    break;
                case "reachable":
                    snakeh = new SnakeReachableHeuristic();
                    break;
                default:
                    Log.WriteLineIf("Snake heuristic: " + splitedArgs["snakeh"] + " is not supported!", TraceLevel.Error);
                    return;
                    break;
            }
            
            if (!splitedArgs.ContainsKey("timelimit")) //default snakeh
            {
                splitedArgs.Add("timelimit", "120");
            }
            int timelimit = Int32.Parse(splitedArgs["timelimit"]);

            if (splitedArgs["problem"].Equals("snake"))
            {
                initState = new Snake(w,Int32.Parse(splitedArgs["s0"]), snakeh);
            } else if (splitedArgs["problem"].Equals("box"))
            {
                List<int> heads = new List<int>();
                int i = 0;
                while (splitedArgs.ContainsKey("s"+i))
                {
                    heads.Add(Int32.Parse(splitedArgs["s" + i]));
                    i++;
                }
                initState = new BoxCartesian(w,heads.ToArray(), boxh, snakeh);
            }
            else if (splitedArgs["problem"].Equals("box-od"))
            {
                List<int> heads = new List<int>();
                int i = 0;
                while (splitedArgs.ContainsKey("s" + i))
                {
                    heads.Add(Int32.Parse(splitedArgs["s" + i]));
                    i++;
                }
                initState = new BoxOD(w, heads.ToArray(),boxh, snakeh);
            }
            else
            {
                Log.WriteLineIf("Problem: " + splitedArgs["problem"] + " is not supported!", TraceLevel.Error);
                return;
            }


            switch (splitedArgs["alg"])
            {
                case "astar":
                    solver = new AStarMax(initState);
                    break;
                case "dfbnb":
                    solver = new DfBnbMax(initState);
                    break;
                default:
                    Log.WriteLineIf("Solver algorithm: " + splitedArgs["alg"] + " is not supported!", TraceLevel.Error);
                    return;
                    break;
            }

            Log.WriteLineIf(@"Solviong snakes in the box problem:", TraceLevel.Info);
            Log.WriteLineIf(@"[[Algorithm:" + solver.GetType().Name + "]]", TraceLevel.Info);
            Log.WriteLineIf(@"[[Problem:" + splitedArgs["problem"] + "]]", TraceLevel.Info);
            Log.WriteLineIf(@"[[WorldDimentions:" + n + "]]", TraceLevel.Info);
            Log.WriteLineIf(@"[[SnakeSpread:" + sk + "]]", TraceLevel.Info);
            Log.WriteLineIf(@"[[BoxSpread:" + bk + "]]", TraceLevel.Info);
            Log.WriteLineIf(@"[[SnakeHeuristics:" + snakeh.GetType().Name + "]]", TraceLevel.Info);
            Log.WriteLineIf(@"[[BoxHeuristics:" + boxh.GetType().Name + "]]", TraceLevel.Info);


            var startTime = DateTime.Now;
            var howEnded = solver.Run(timelimit);
            var totalTime = DateTime.Now - startTime;
            var goal = (ISibNode) solver.GetMaxGoal();
            Log.WriteLineIf("[[TotalTime(MS):" + totalTime.TotalMilliseconds + "]]", TraceLevel.Off);
            Log.WriteLineIf("[[Expended:" + solver.Expended + "]]", TraceLevel.Off);
            Log.WriteLineIf("[[Generated:" + solver.Generated + "]]", TraceLevel.Off);
            Log.WriteLineIf("[[Pruned:" + solver.Pruned + "]]", TraceLevel.Off);
            Log.WriteLineIf("[[G-Value:" + goal.g + "]]", TraceLevel.Off);
            Log.WriteLineIf("[[GoalBits:" + goal.GetBitsString() + "]]", TraceLevel.Off);
            Log.WriteLineIf("[[Goal:" + goal.GetIntString() + "]]", TraceLevel.Off);
            Log.WriteLineIf("[[HowEnded:" + Enum.GetName(typeof(State), howEnded) + "]]", TraceLevel.Off);
            var snakeFreeSpots = goal.GetSnakeSpreadFreeSpots();
            Log.WriteLineIf("[[SnakeSpreadFreeSpotsCount:" + snakeFreeSpots.Count + "]]", TraceLevel.Off);
            Log.WriteLineIf("[[SnakeSpreadFreeSpotsPlaces:" + string.Join("-", snakeFreeSpots) + "]]", TraceLevel.Off);
            if (goal is Box)
            {
                var boxFreeSpots = ((Box)goal).GetBoxSpreadFreeSpots();
                Log.WriteLineIf("[[BoxSpreadFreeSpotsCount:" + boxFreeSpots.Count  + "]]", TraceLevel.Off);
                Log.WriteLineIf("[[BoxSpreadFreeSpotsPlaces:" + string.Join("-",boxFreeSpots)  + "]]", TraceLevel.Off);
            }

            var sLoop = 0;
            while (splitedArgs.ContainsKey("s" + sLoop))
            {
                Log.WriteLineIf("[[S" + sLoop + ":" + splitedArgs["s" + sLoop] + "]]", TraceLevel.Info);
                sLoop++;
            }
        }

        private static void MemTest()
        {
            //MEMBLAST
            System.Console.WriteLine(@"Filling memory to check 64bit pointers - kill me when you want");
            List<Snake> m = new List<Snake>();
            long c = 0;
            while (true)
            {
                m.Add(new Snake(new World(8, 2), 0, new SnakeNoneHeuristic(), false));
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
