
using System;
using System.Collections.Generic;
using System.IO;
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
            _worlds.AddLast(new World(File.ReadAllText(@"..\..\33.grd"), new UntouchedAroundTheGoalHeuristic()));
            _worlds.AddLast(new World(File.ReadAllText(@"..\..\Clean_Grid_5x5.grd"), new UntouchedAroundTheGoalHeuristic()));
            _worlds.AddLast(new World(File.ReadAllText(@"..\..\Clean_Grid_5x5BasicBlocked.grd"), new UntouchedAroundTheGoalHeuristic()));
            _worlds.AddLast(new World(File.ReadAllText(@"..\..\Grid-10-6-5-4-0.grd"), new UntouchedAroundTheGoalHeuristic()));
            _worlds.AddLast(new World(File.ReadAllText(@"..\..\Grid-10-6-5-4-1.grd"), new UntouchedAroundTheGoalHeuristic()));
            _worlds.AddLast(new World(File.ReadAllText(@"..\..\Grid-20-4-4-2-16.grd"), new UntouchedAroundTheGoalHeuristic()));
            _worlds.AddLast(new World(File.ReadAllText(@"..\..\Grid-40-4-4-3-2.grd"), new UntouchedAroundTheGoalHeuristic()));
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
        
    }
}
