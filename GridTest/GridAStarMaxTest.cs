
using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Grid.Domain;
using MaxSearchAlg;

namespace GridTest
{
    [TestClass]
    public class GridAStarMaxTest
    {
        private static World _basicWorld;
        private static World _basicWorld2;
        private static World _basicWorld3;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _basicWorld = new World(File.ReadAllText(@"..\..\33.grd"), new UntouchedAroundTheGoalHeuristic());
            _basicWorld2 = new World(File.ReadAllText(@"..\..\Grid-10-6-5-4-1.grd"), new UntouchedAroundTheGoalHeuristic());
            _basicWorld3 = new World(File.ReadAllText(@"..\..\Clean_Grid_5x5BasicBlocked.grd"), new UntouchedAroundTheGoalHeuristic());
        }

        [TestMethod]
        public void Create_NewAStarGrid3X3_findPath()
        {
            GridSearchNode initialState = _basicWorld.GetInitialSearchNode();
            AStarMax astar = new AStarMax(initialState);
            Assert.IsNotNull(astar);
            astar.Run(Int32.MaxValue); //Prevent stoping by time, should stop only when goal found
            var maxGoal = astar.GetMaxGoal();
            Assert.AreEqual(8, maxGoal.g);
        }

        [TestMethod]
        public void Create_NewAStarGrid5X5WithBlocked_FindPath()
        {
            GridSearchNode initialState = _basicWorld3.GetInitialSearchNode();
            AStarMax astar = new AStarMax(initialState);
            Assert.IsNotNull(astar);
            astar.Run(Int32.MaxValue);
            var maxGoal = astar.GetMaxGoal();
            Assert.AreEqual(20, maxGoal.g);
        }
    }
}
