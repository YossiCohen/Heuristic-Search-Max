using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Common;
using Grid.Domain;
using MaxSearchAlg;

namespace Grid
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine(@"Please Provide arguments to run:");
                Console.WriteLine(@"all args should be in the form of: [key]=[value] with space between them");
                Console.WriteLine(@"Arguments:");
                Console.WriteLine(@"----------");
                Console.WriteLine(@"problem:     problem filename");
                Console.WriteLine(@"timeLimit:   limit run time to X minutes (default 120), 0 for no time limit");
                return;
            }

            Dictionary<string, string> splitedArgs = SplitArguments(args);
            if (splitedArgs.ContainsKey("memtest") && splitedArgs["memtest"].Equals("true"))
            {
                MemTest();
            }
            if (!splitedArgs.ContainsKey("timelimit")) //default snakeh
            {
                splitedArgs.Add("timelimit", "120");
            }
            int timelimit = Int32.Parse(splitedArgs["timelimit"]);

            string problemFileName = splitedArgs["problem"];
            World world = new World(File.ReadAllText(problemFileName), new UntouchedAroundTheGoalHeuristic());
            AStarMax solver = new AStarMax(world.GetInitialSearchNode());
            solver.Run(timelimit);

            Log.WriteLineIf(@"Solviong snakes in the box problem:", TraceLevel.Info);
            Log.WriteLineIf(@"[[Problem:" + problemFileName + "]]", TraceLevel.Info);

            var startTime = DateTime.Now;
            var howEnded = solver.Run(timelimit);
            var totalTime = DateTime.Now - startTime;
            var goal = (GridSearchNode)solver.GetMaxGoal();
            Log.WriteLineIf("[[TotalTime(MS):" + totalTime.TotalMilliseconds + "]]", TraceLevel.Off);
            Log.WriteLineIf("[[Expended:" + solver.Expended + "]]", TraceLevel.Off);
            Log.WriteLineIf("[[Generated:" + solver.Generated + "]]", TraceLevel.Off);
            Log.WriteLineIf("[[Pruned:" + solver.Pruned + "]]", TraceLevel.Off);
            Log.WriteLineIf("[[G-Value:" + goal.g + "]]", TraceLevel.Off);
            Log.WriteLineIf("[[GoalBits:" + goal.GetBitsString() + "]]", TraceLevel.Off);
            Log.WriteLineIf("[[Goal:" + goal.GetNodeStringV2() + "]]", TraceLevel.Off);
            Log.WriteLineIf("[[HowEnded:" + Enum.GetName(typeof(State), howEnded) + "]]", TraceLevel.Off);

        }

        private static void MemTest()
        {
            //MEMBLAST
            Console.WriteLine(@"Filling memory to check 64bit pointers - kill me when you want");
            List<String> m = new List<String>();
            long c = 0;
            while (true)
            {
                m.Add(Guid.NewGuid().ToString());
                c++;
                if (c % 1000000 == 0)
                {
                    Console.WriteLine(c + "toalmem:" + GC.GetTotalMemory(false) + " ListCount:" + m.Count);
                }
            }
            // ReSharper disable once FunctionNeverReturns
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