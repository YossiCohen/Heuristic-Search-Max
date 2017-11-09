
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
        private static World _basicWorld3x3;
        private static World _basicWorld5x5Blocked;
        private static World _basicWorld6x5_0;
        private static World _basicWorld6x5_1;
        private static World _basicWorld4x4_16;



        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _basicWorld3x3 = new World(File.ReadAllText(@"..\..\33.grd"), new UntouchedAroundTheGoalHeuristic());
            _basicWorld5x5Blocked = new World(File.ReadAllText(@"..\..\Clean_Grid_5x5BasicBlocked.grd"), new UntouchedAroundTheGoalHeuristic());
            _basicWorld6x5_0 = new World(File.ReadAllText(@"..\..\Grid-10-6-5-4-0.grd"), new UntouchedAroundTheGoalHeuristic());
            _basicWorld6x5_1 = new World(File.ReadAllText(@"..\..\Grid-10-6-5-4-1.grd"), new UntouchedAroundTheGoalHeuristic());
            _basicWorld4x4_16 = new World(File.ReadAllText(@"..\..\Grid-20-4-4-2-16.grd"), new UntouchedAroundTheGoalHeuristic());
        }

        [TestMethod]
        public void Create_NewAStarGrid3X3_findPath()
        {
            GridSearchNode initialState = _basicWorld3x3.GetInitialSearchNode();
            AStarMax astar = new AStarMax(initialState);
            Assert.IsNotNull(astar);
            astar.Run(Int32.MaxValue); //Prevent stoping by time, should stop only when goal found
            var maxGoal = astar.GetMaxGoal();
            Assert.AreEqual(8, maxGoal.g);
        }

        [TestMethod]
        public void Create_NewAStarGrid5X5WithBlocked_FindPath()
        {
            GridSearchNode initialState = _basicWorld5x5Blocked.GetInitialSearchNode();
            AStarMax astar = new AStarMax(initialState);
            Assert.IsNotNull(astar);
            astar.Run(Int32.MaxValue);
            var maxGoal = astar.GetMaxGoal();
            Assert.AreEqual(20, maxGoal.g);
        }

        [TestMethod]
        public void Create_NewAStarGrid6X5WithBlocked0_FindPath()
        {
            GridSearchNode initialState = _basicWorld6x5_0.GetInitialSearchNode();
            AStarMax astar = new AStarMax(initialState);
            Assert.IsNotNull(astar);
            astar.Run(Int32.MaxValue);
            var maxGoal = astar.GetMaxGoal();
            Assert.AreEqual(23, maxGoal.g);
        }

        [TestMethod]
        public void Create_NewAStarGrid6X5WithBlocked1_FindPath()
        {
            GridSearchNode initialState = _basicWorld6x5_1.GetInitialSearchNode();
            AStarMax astar = new AStarMax(initialState);
            Assert.IsNotNull(astar);
            astar.Run(Int32.MaxValue);
            var maxGoal = astar.GetMaxGoal();
            Assert.AreEqual(22, maxGoal.g);
        }

        [TestMethod]
        public void Create_NewAStarGrid4X4WithBlocked16_FindPath()
        {
            GridSearchNode initialState = _basicWorld4x4_16.GetInitialSearchNode();
            AStarMax astar = new AStarMax(initialState);
            Assert.IsNotNull(astar);
            astar.Run(Int32.MaxValue);
            var maxGoal = astar.GetMaxGoal();
            Assert.AreEqual(10, maxGoal.g);
        }

    }
}
