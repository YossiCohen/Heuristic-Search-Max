
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Grid.Domain;
using MaxSearchAlg;

namespace GridTest
{
    [TestClass]
    public class SpecialIntrestingCases
    {

        private static LinkedList<World> _worlds;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _worlds = new LinkedList<World>();
            _worlds.AddLast(new World(File.ReadAllText(@"..\..\33.grd"), new UntouchedAroundTheGoalHeuristic(), WorldType.Uniform));
            _worlds.AddLast(new World(File.ReadAllText(@"..\..\Clean_Grid_5x5.grd"), new UntouchedAroundTheGoalHeuristic(), WorldType.Uniform));
            _worlds.AddLast(new World(File.ReadAllText(@"..\..\Clean_Grid_5x5BasicBlocked.grd"), new UntouchedAroundTheGoalHeuristic(), WorldType.Uniform));
            _worlds.AddLast(new World(File.ReadAllText(@"..\..\Grid-10-6-5-4-0.grd"), new UntouchedAroundTheGoalHeuristic(), WorldType.Uniform));
            _worlds.AddLast(new World(File.ReadAllText(@"..\..\Grid-10-6-5-4-1.grd"), new UntouchedAroundTheGoalHeuristic(), WorldType.Uniform));
            _worlds.AddLast(new World(File.ReadAllText(@"..\..\Grid-20-4-4-2-16.grd"), new UntouchedAroundTheGoalHeuristic(), WorldType.Uniform));
            _worlds.AddLast(new World(File.ReadAllText(@"..\..\Grid-40-4-4-3-2.grd"), new UntouchedAroundTheGoalHeuristic(), WorldType.Uniform));
        }

        [TestMethod]
        public void Compare_AStar_To_DfBNB()
        {
            foreach (var world in _worlds)
            {
                AStarMax astar = new AStarMax(world.GetInitialSearchNode<GridSearchNode>(), new GoalOnLocation(world.Goal));
                Assert.IsNotNull(astar);
                astar.Run(Int32.MaxValue);
                var AstarMaxGoal = astar.GetMaxGoal();
                DfBnbMax dfbnb = new DfBnbMax(world.GetInitialSearchNode<GridSearchNode>(), new GoalOnLocation(world.Goal));
                Assert.IsNotNull(dfbnb);
                dfbnb.Run(Int32.MaxValue);
                var DfbnbMaxGoal = dfbnb.GetMaxGoal();
                Assert.AreEqual(AstarMaxGoal.g, DfbnbMaxGoal.g);
            }
        }

        //TODO: Add test: foreach:
        //[Uniform/Life]
        //Compare that all below get the same Goal!
        //[A*,DFBNB]
        //[Heuristics]
        //[Pruning]
        //


        [TestMethod]
        public void HugeCompare_ALG_HEUR_PRUN_PER_DOMAIN()
        {
            var sanityFiles = Directory.GetFiles(@"..\..\SanityGrids\","*.grd");
            var solvers = new string[] { "AStarMax","DfBnbMax" };
            //none/untouched/bcc/alternate/altbcc/sepaltbcc
            var heuristics = new string[]
            {   //"NoneHeuristic",
                "UntouchedAroundTheGoalHeuristic",
                "BiconnectedComponentsHeuristic",
                "AlternateStepsHeuristic",
                "AlternateStepsBiconnectedComponentsHeuristic",
                "SeparateAlternateStepsBiconnectedComponentsHeuristic"
            };
            var prunings = new string[]
            {
                "NoPrunning",
                "BasicSymmetryDetectionPrunning",
                "HashedBasicSymmetryDetectionPrunning",
                "ReachableSymmetryDetectionPrunning"
            };
            IGridHeuristic heuristic;
            foreach (var sanityFile in sanityFiles)
            {

                foreach (WorldType wrldType in (WorldType[])Enum.GetValues(typeof(WorldType)))
                {
                    if (wrldType == WorldType.Uniform) continue; //TODO - enable this when life works
                    int maxLength = -1;
                    foreach (var solverName in solvers)
                    {
                        foreach (var pruneName in prunings)
                        {
                            if (solverName == "DfBnbMax" && pruneName == "ReachableSymmetryDetectionPrunning") continue;
                            if (wrldType == WorldType.Life && pruneName == "ReachableSymmetryDetectionPrunning") continue;

                            foreach (var heuristicName in heuristics)
                            {
                                IPrunningMethod prune;
                                GridSearchNode initialNode;

                                if (heuristicName == "NoneHeuristic")
                                {
                                    heuristic = new NoneHeuristic();
                                    if (pruneName == "ReachableSymmetryDetectionPrunning")
                                    {
                                        //Some issues here - we will pass this test
                                        continue;
                                    }
                                }
                                else if (heuristicName == "UntouchedAroundTheGoalHeuristic")
                                {
                                    if (pruneName == "ReachableSymmetryDetectionPrunning")
                                    {
                                        heuristic = new RsdUntouchedAroundTheGoalHeuristic();
                                    }
                                    else
                                    {
                                        heuristic = new UntouchedAroundTheGoalHeuristic();
                                    }
                                }
                                else if (heuristicName == "BiconnectedComponentsHeuristic")
                                {  //TODO: finish impl. + support RSD
                                    heuristic = new BiconnectedComponentsHeuristic();
                                }
                                else if (heuristicName == "AlternateStepsHeuristic")
                                {
                                    heuristic = new AlternateStepsHeuristic();
                                }
                                else if (heuristicName == "AlternateStepsBiconnectedComponentsHeuristic")
                                {
                                    heuristic = new AlternateStepsBiconnectedComponentsHeuristic();
                                }
                                else if (heuristicName == "SeparateAlternateStepsBiconnectedComponentsHeuristic")
                                {
                                    heuristic = new SeparateAlternateStepsBiconnectedComponentsHeuristic();
                                }
                                else
                                {
                                    throw new Exception("heuristic not found");
                                }

                                World world = new World(File.ReadAllText(sanityFile), heuristic, wrldType);
                                

                                switch (pruneName)
                                {
                                    case "NoPrunning":
                                        prune = new NoPrunning();
                                        initialNode = world.GetInitialSearchNode<GridSearchNode>();
                                        break;
                                    case "BasicSymmetryDetectionPrunning":
                                        prune = new BasicSymmetryDetectionPrunning();
                                        initialNode = world.GetInitialSearchNode<GridSearchNode>();
                                        break;
                                    case "HashedBasicSymmetryDetectionPrunning":
                                        prune = new HashedBasicSymmetryDetectionPrunning();
                                        initialNode = world.GetInitialSearchNode<GridSearchNode>();
                                        break;
                                    case "ReachableSymmetryDetectionPrunning":
                                        prune = new ReachableSymmetryDetectionPrunning();
                                        initialNode = world.GetInitialSearchNode<RsdGridSearchNode>();
                                        break;
                                    default:
                                        return;
                                }

                                Solver solver;
                                switch (solverName)
                                {
                                    case "AStarMax":
                                        solver = new AStarMax(initialNode, prune, new GoalOnLocation(world.Goal));
                                        break;
                                    case "DfBnbMax":
                                        solver = new DfBnbMax(initialNode, prune, new GoalOnLocation(world.Goal));
                                        break;
                                    default:
                                        return;
                                }

                                if (pruneName == "ReachableSymmetryDetectionPrunning")
                                {
                                    //Sorry but RSD must use AStarMax - DFBnB not supported
                                    ((ReachableSymmetryDetectionPrunning)prune).setAstarOpenList(((AStarMax)solver).OpenList);
                                }

                                Log.WriteLineIf(string.Format("[File:{0},WorldType:{1},SearchAlg:{2},Heuristic:{3},Pruning:{4}]",sanityFile.Substring(18),wrldType,solverName,heuristicName,pruneName),TraceLevel.Off);
                                var howEnded = solver.Run(0);
                                Assert.AreEqual(howEnded, State.Ended);
                                var goal = solver.GetMaxGoal().g;
                                if (maxLength == -1)
                                {
                                    maxLength = goal;
                                }
                                else
                                {
                                    Assert.AreEqual(maxLength, goal);
                                }
                                
                            }

                        }

                    }

                }
            }
;

        }

    }
}
