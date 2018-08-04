﻿using System;
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
                Console.WriteLine(@"alg:         [astar/dfbnb] the solving algorithm");
                Console.WriteLine(@"prune:       [none/bsd/rsd] the solving algorithm");
                Console.WriteLine(@"initBCC:     [true/false] remove non-reachable areas from the graph on init");
                Console.WriteLine(@"----------");
                Console.WriteLine(@"memTest:     if set to true, will not solve nothing, only fill memory");
                Console.WriteLine(@"             allocation to check 64bit issue");
                return;
            }

            Dictionary<string, string> splitedArgs = SplitArguments(args);
            if (splitedArgs.ContainsKey("memtest") && splitedArgs["memtest"].Equals("true"))
            {
                MemTest();
            }
            if (!splitedArgs.ContainsKey("timelimit")) //default time limit
            {
                splitedArgs.Add("timelimit", "120");
            }
            if (!splitedArgs.ContainsKey("initbcc")) //default pre-bcc
            {
                splitedArgs.Add("initbcc", "false");
            }
            int timelimit = Int32.Parse(splitedArgs["timelimit"]);
            bool initWithBCC = Boolean.Parse(splitedArgs["initbcc"]);

            string problemFileName = splitedArgs["problem"];

            World world;
            if (splitedArgs["prune"] == "rsd")
            {
                world = new World(File.ReadAllText(problemFileName), new RsdUntouchedAroundTheGoalHeuristic());
            }
            else
            {
                world = new World(File.ReadAllText(problemFileName), new UntouchedAroundTheGoalHeuristic());
            }

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
                default:
                    Log.WriteLineIf("Solver algorithm: " + splitedArgs["alg"] + " is not supported!", TraceLevel.Error);
                    return;
            }

            if (splitedArgs["prune"] == "rsd")
            {
                //Sorry but RSD must use AStarMax
                ((ReachableSymmetryDetectionPrunning)prune).setAstarOpenList(((AStarMax)solver).OpenList);
            }

            if (initWithBCC)
            {
                world.InitBcc();
            }

            Log.WriteLineIf(@"Solviong 2D-Grid problem from file:", TraceLevel.Off);
            Log.WriteLineIf(@"[[Problem:" + problemFileName + "]]", TraceLevel.Off);
            Log.WriteLineIf(@"[[Algorithm:" + solver.GetType().Name + "]]", TraceLevel.Off);
            Log.WriteLineIf(@"[[Prunning:" + prune.GetType().Name + "]]", TraceLevel.Off);
            Log.WriteLineIf(@"[[InitWithBcc:" + initWithBCC + "]]", TraceLevel.Off);

            var startTime = DateTime.Now;
            var howEnded = solver.Run(timelimit);
            var totalTime = DateTime.Now - startTime;
            var goal = (GridSearchNode)solver.GetMaxGoal();

            Log.WriteLineIf("[[TotalTime(MS):" + totalTime.TotalMilliseconds + "]]", TraceLevel.Off);
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