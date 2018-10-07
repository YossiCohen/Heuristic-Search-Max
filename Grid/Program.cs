using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using Common;
using Grid.Domain;
using MaxSearchAlg;

namespace Grid
{
    class Program
    {

        private static readonly string VERSION = "1.8";
        private static readonly string TIME_LIMIT = ConfigurationSettings.AppSettings["TimeLimit"] == null ? "120" : ConfigurationSettings.AppSettings["TimeLimit"];
        private static readonly string BCC_INIT = ConfigurationSettings.AppSettings["BccInit"] == null ? "true" : ConfigurationSettings.AppSettings["BccInit"];

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine(@"Please Provide arguments to run:");
                Console.WriteLine(@"all args should be in the form of: [key]=[value] with space between them");
                Console.WriteLine(@"Arguments:");
                Console.WriteLine(@"----------");
                Console.WriteLine(@"problem:     problem filename");
                Console.WriteLine(@"time-limit:  limit run time to X minutes (default 120), 0 for no time limit");
                Console.WriteLine(@"alg:         [astar/dfbnb/greedy/greedyloops] the solving algorithm");
                Console.WriteLine(@"heuristic:   [none/untouched/bcc/alternate/altbcc] the heuristic being used");
                Console.WriteLine(@"prune:       [none/bsd/rsd] pruning technique");
                Console.WriteLine(@"bcc-init:    [true/false] remove non-reachable areas from the graph on init");
                Console.WriteLine(@"----------");
                Console.WriteLine(@"memTest:     if set to true, will not solve nothing, only fill memory");
                Console.WriteLine(@"             allocation to check 64bit issue");
                Console.WriteLine(@"-----------------------------[Version:"+VERSION+"]---------------------------------");
                Console.WriteLine(@"time-limit & bcc-init can be set in app.config XML file");
                return;
            }

            Dictionary<string, string> splitedArgs = ConsoleAppHelper.SplitArguments(args);
            if (splitedArgs.ContainsKey("memtest") && splitedArgs["memtest"].Equals("true"))
            {
                MemTest();
            }
            if (!splitedArgs.ContainsKey("time-limit")) //default time limit
            {
                splitedArgs.Add("time-limit", TIME_LIMIT);
            }

            if (!splitedArgs.ContainsKey("bcc-init")) //default pre-bcc
            {
                splitedArgs.Add("bcc-init", BCC_INIT);
            }

            int timelimit = Int32.Parse(splitedArgs["time-limit"]);
            bool bccInit = Boolean.Parse(splitedArgs["bcc-init"]);

            string problemFileName = splitedArgs["problem"];

            IGridHeuristic heuristic;
            if (splitedArgs["heuristic"] == "none")
            {
                heuristic = new NoneHeuristic();
            }
            else if (splitedArgs["heuristic"] == "untouched")
            {
                if (splitedArgs["prune"] == "rsd")
                {
                    heuristic = new RsdUntouchedAroundTheGoalHeuristic();
                }
                else
                {
                    heuristic = new UntouchedAroundTheGoalHeuristic();
                }
            }
            else if (splitedArgs["heuristic"] == "bcc")
            {  //TODO: finish impl. + support RSD
                heuristic = new BiconnectedComponentsHeuristic();
            }
            else if (splitedArgs["heuristic"] == "alternate")
            {
                heuristic = new AlternateStepsHeuristic();
            }
            else if(splitedArgs["heuristic"] == "altbcc")
            {
                heuristic = new AlternateStepsBiconnectedComponentsHeuristic();
            }
            else
            {
                throw new NotImplementedException();
            }

            World world = new World(File.ReadAllText(problemFileName), heuristic);

            IPrunningMethod prune;
            GridSearchNode initialNode;
            switch (splitedArgs["prune"])
            {
                case "none":
                    prune = new NoPrunning();
                    initialNode = world.GetInitialSearchNode<GridSearchNode>();
                    break;
                case "bsd":
                    prune = new BasicSymmetryDetectionPrunning();
                    initialNode = world.GetInitialSearchNode<GridSearchNode>();
                    break;
                case "rsd":
                    prune = new ReachableSymmetryDetectionPrunning();
                    initialNode = world.GetInitialSearchNode<RsdGridSearchNode>();
                    break;
                default:
                    Log.WriteLineIf("Prunning Method: " + splitedArgs["prune"] + " is not supported!", TraceLevel.Error);
                    return;
            }

            Solver solver;
            switch (splitedArgs["alg"])
            {
                case "astar":
                    solver = new AStarMax(initialNode, prune, new GoalOnLocation(world.Goal));
                    break;
                case "dfbnb":
                    solver = new DfBnbMax(initialNode, prune, new GoalOnLocation(world.Goal));
                    break;
                case "greedy":
                    solver = new GreedyMax(initialNode, new GoalOnLocation(world.Goal));
                    if (splitedArgs["prune"] != "none")
                    {
                        Log.WriteLineIf("Greedy doesn't support pruning!", TraceLevel.Error);
                        return;
                    }
                    break;
                case "greedyloops":
                    solver = new GreedyLoopMax(initialNode, new GoalOnLocation(world.Goal),50);
                    if (splitedArgs["prune"] != "none")
                    {
                        Log.WriteLineIf("GreedyLoops doesn't support pruning!", TraceLevel.Error);
                        return;
                    }
                    break;
                default:
                    Log.WriteLineIf("Solver algorithm: " + splitedArgs["alg"] + " is not supported!", TraceLevel.Error);
                    return;
            }

            if (splitedArgs["prune"] == "rsd")
            {
                //Sorry but RSD must use AStarMax - DFBnB not supported
                ((ReachableSymmetryDetectionPrunning)prune).setAstarOpenList(((AStarMax)solver).OpenList);
            }

            if (bccInit)
            {
                world.InitBcc();
            }

            Log.WriteLineIf(@"Solviong 2D-Grid problem from file:", TraceLevel.Off);
            Log.WriteLineIf(@"[[Problem:" + problemFileName + "]]", TraceLevel.Off);
            Log.WriteLineIf(@"[[Algorithm:" + solver.GetType().Name + "]]", TraceLevel.Off);
            Log.WriteLineIf(@"[[Heuristic:" + heuristic.GetName() + "]]", TraceLevel.Off);
            Log.WriteLineIf(@"[[Prunning:" + prune.GetType().Name + "]]", TraceLevel.Off);
            Log.WriteLineIf(@"[[BccInit:" + bccInit + "]]", TraceLevel.Off);
            Log.WriteLineIf(@"[[TimeLimit:" + timelimit + "]]", TraceLevel.Off);
            Log.WriteLineIf(@"[[Width:" + world.Width + "]]", TraceLevel.Off);
            Log.WriteLineIf(@"[[Height:" + world.Height + "]]", TraceLevel.Off);
            Log.WriteLineIf(@"[[BlockedCount:" + world.TotalBlocked + "]]", TraceLevel.Off);
            Log.WriteLineIf(@"[[NumberOfEvenLocations:" + AlternateStepsHeuristic.GetNumberOfEvenLocations(world.Width, world.Height) + "]]", TraceLevel.Off);
            Log.WriteLineIf(@"[[NumberOfOddLocations:" + AlternateStepsHeuristic.GetNumberOfOddLocations(world.Width, world.Height) + "]]", TraceLevel.Off);
            Log.WriteLineIf(@"[[EvenBlockedCount:" + world.EvenBlocked + "]]", TraceLevel.Off);
            Log.WriteLineIf(@"[[OddBlockedCount:" + world.OddBlocked + "]]", TraceLevel.Off);
            Log.WriteLineIf(@"[[EvenFreeCount:" + (AlternateStepsHeuristic.GetNumberOfEvenLocations(world.Width, world.Height) - world.EvenBlocked) + "]]", TraceLevel.Off);
            Log.WriteLineIf(@"[[OddFreeCount:" + (AlternateStepsHeuristic.GetNumberOfOddLocations(world.Width, world.Height) - world.OddBlocked) + "]]", TraceLevel.Off);

            Log.WriteLineIf(@"[[EvenLocationsPercent:" + (decimal)AlternateStepsHeuristic.GetNumberOfEvenLocations(world.Width, world.Height)/world.LinearSize + "]]", TraceLevel.Off);
            Log.WriteLineIf(@"[[OddLocationsPercent:" + (decimal)AlternateStepsHeuristic.GetNumberOfOddLocations(world.Width, world.Height) / world.LinearSize + "]]", TraceLevel.Off);
            Log.WriteLineIf(@"[[EvenBlockedPercent:" + (decimal)world.EvenBlocked / world.LinearSize + "]]", TraceLevel.Off);
            Log.WriteLineIf(@"[[OddBlockedPercent:" + (decimal)world.OddBlocked / world.LinearSize + "]]", TraceLevel.Off);
            Log.WriteLineIf(@"[[EvenFreePercent:" + (decimal)(AlternateStepsHeuristic.GetNumberOfEvenLocations(world.Width, world.Height) - world.EvenBlocked) / world.LinearSize + "]]", TraceLevel.Off);
            Log.WriteLineIf(@"[[OddFreePercent:" + (decimal)(AlternateStepsHeuristic.GetNumberOfOddLocations(world.Width, world.Height) - world.OddBlocked) / world.LinearSize + "]]", TraceLevel.Off);

            var startTime = DateTime.Now;
            ulong startCycles = NativeMethods.GetThreadCycles();

            var howEnded = solver.Run(timelimit);

            ulong totalCycles = NativeMethods.GetThreadCycles() - startCycles;
            var totalTime = DateTime.Now - startTime;
            var goal = (GridSearchNode)solver.GetMaxGoal();

            Log.WriteLineIf("[[TotalTime(MS):" + totalTime.TotalMilliseconds + "]]", TraceLevel.Off);
            Log.WriteLineIf("[[CPU Cycles:" + totalCycles + "]]", TraceLevel.Off);
            Log.WriteLineIf("[[Expended:" + solver.Expended + "]]", TraceLevel.Off);
            Log.WriteLineIf("[[Generated:" + solver.Generated + "]]", TraceLevel.Off);
            Log.WriteLineIf("[[Pruned:" + solver.Pruned + "]]", TraceLevel.Off);
            if (goal != null)
            {
                Log.WriteLineIf("[[G-Value:" + goal.g + "]]", TraceLevel.Off);
                Log.WriteLineIf("[[GoalBits:" + goal.GetBitsString() + "]]", TraceLevel.Off);
                Log.WriteLineIf("[[Goal:" + goal.GetNodeStringV2() + "]]", TraceLevel.Off);
            }
            else
            {
                Log.WriteLineIf("[[G-Value:" + -1 + "]]", TraceLevel.Off);
                Log.WriteLineIf("[[GoalBits:NOGOAL]]", TraceLevel.Off);
                Log.WriteLineIf("[[Goal:NOGOAL]]", TraceLevel.Off);
            }
            Log.WriteLineIf("[[HowEnded:" + Enum.GetName(typeof(State), howEnded) + "]]", TraceLevel.Off);
            Log.WriteLineIf("[[GridSolverVersion:"+VERSION+"]]", TraceLevel.Off);
        }

        private static void MemTest()
        {
            //MEMBLAST
            Log.WriteLineIf("MEMBLAST", TraceLevel.Off);
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
    }
}