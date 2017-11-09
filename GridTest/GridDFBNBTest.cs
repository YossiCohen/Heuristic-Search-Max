
using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Grid.Domain;
using MaxSearchAlg;

namespace GridTest
{
    [TestClass]
    public class GridDfbnbTest
    {
        private static World _basicWorld3X3;
        private static World _basicWorld5X5Blocked;
        private static World _basicWorld6X50;
        private static World _basicWorld6X51;
        private static World _basicWorld4X416;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _basicWorld3X3 = new World(File.ReadAllText(@"..\..\33.grd"), new UntouchedAroundTheGoalHeuristic());
            _basicWorld5X5Blocked = new World(File.ReadAllText(@"..\..\Clean_Grid_5x5BasicBlocked.grd"), new UntouchedAroundTheGoalHeuristic());
            _basicWorld6X50 = new World(File.ReadAllText(@"..\..\Grid-10-6-5-4-0.grd"), new UntouchedAroundTheGoalHeuristic());
            _basicWorld6X51 = new World(File.ReadAllText(@"..\..\Grid-10-6-5-4-1.grd"), new UntouchedAroundTheGoalHeuristic());
            _basicWorld4X416 = new World(File.ReadAllText(@"..\..\Grid-20-4-4-2-16.grd"), new UntouchedAroundTheGoalHeuristic());
        }

        [TestMethod]
        public void Create_NewDfBnbGrid3X3_findPath()
        {
            GridSearchNode initialState = _basicWorld3X3.GetInitialSearchNode();
            Solver solver = new DfBnbMax(initialState);
            Assert.IsNotNull(solver);
            solver.Run(Int32.MaxValue); //Prevent stoping by time, should stop only when goal found
            var maxGoal = solver.GetMaxGoal();
            Assert.AreEqual(8, maxGoal.g);
        }

        [TestMethod]
        public void Create_NewDfBnbGrid5X5WithBlocked_FindPath()
        {
            GridSearchNode initialState = _basicWorld5X5Blocked.GetInitialSearchNode();
            Solver solver = new DfBnbMax(initialState);
            Assert.IsNotNull(solver);
            solver.Run(Int32.MaxValue);
            var maxGoal = solver.GetMaxGoal();
            Assert.AreEqual(20, maxGoal.g);
        }

        [TestMethod]
        public void Create_NewDfBnbGrid6X5WithBlocked0_FindPath()
        {
            GridSearchNode initialState = _basicWorld6X50.GetInitialSearchNode();
            Solver solver = new DfBnbMax(initialState);
            Assert.IsNotNull(solver);
            solver.Run(Int32.MaxValue);
            var maxGoal = solver.GetMaxGoal();
            Assert.AreEqual(23, maxGoal.g);
        }

        [TestMethod]
        public void Create_NewDfBnbGrid6X5WithBlocked1_FindPath()
        {
            GridSearchNode initialState = _basicWorld6X51.GetInitialSearchNode();
            Solver solver = new DfBnbMax(initialState);
            Assert.IsNotNull(solver);
            solver.Run(Int32.MaxValue);
            var maxGoal = solver.GetMaxGoal();
            Assert.AreEqual(22, maxGoal.g);
        }

        [TestMethod]
        public void Create_NewDfBnbGrid4X4WithBlocked16_FindPath()
        {
            GridSearchNode initialState = _basicWorld4X416.GetInitialSearchNode();
            Solver solver = new DfBnbMax(initialState);
            Assert.IsNotNull(solver);
            solver.Run(Int32.MaxValue);
            var maxGoal = solver.GetMaxGoal();
            Assert.AreEqual(10, maxGoal.g);
        }

    }
}
