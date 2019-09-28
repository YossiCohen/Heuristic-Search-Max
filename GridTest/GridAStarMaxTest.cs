
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
        private static World _basicWorld3X3;
        private static World _basicWorld5X5Blocked;
        private static World _basicWorld6X50;
        private static World _basicWorld6X51;
        private static World _basicWorld4X416;
        private static World _basicWorld14X14;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _basicWorld3X3 = new World(File.ReadAllText(@"..\..\33.grd"), new UntouchedAroundTheGoalHeuristic());
            _basicWorld5X5Blocked = new World(File.ReadAllText(@"..\..\Clean_Grid_5x5BasicBlocked.grd"), new UntouchedAroundTheGoalHeuristic());
            _basicWorld6X50 = new World(File.ReadAllText(@"..\..\Grid-10-6-5-4-0.grd"), new UntouchedAroundTheGoalHeuristic());
            _basicWorld6X51 = new World(File.ReadAllText(@"..\..\Grid-10-6-5-4-1.grd"), new UntouchedAroundTheGoalHeuristic());
            _basicWorld4X416 = new World(File.ReadAllText(@"..\..\Grid-20-4-4-2-16.grd"), new UntouchedAroundTheGoalHeuristic());
            _basicWorld14X14 = new World(File.ReadAllText(@"..\..\14x14.grd"), new UntouchedAroundTheGoalHeuristic());
        }

        [TestMethod]
        public void Create_NewAStarGrid3X3_findPath()
        {
            GridSearchNode initialState = _basicWorld3X3.GetInitialSearchNode<GridSearchNode>();
            AStarMax astar = new AStarMax(initialState, new GoalOnLocation(_basicWorld3X3.Goal));
            Assert.IsNotNull(astar);
            astar.Run(Int32.MaxValue); //Prevent stoping by time, should stop only when goal found
            var maxGoal = astar.GetMaxGoal();
            Assert.AreEqual(8, maxGoal.g);
        }

        [TestMethod]
        public void Create_NewAStarGrid5X5WithBlocked_FindPath()
        {
            GridSearchNode initialState = _basicWorld5X5Blocked.GetInitialSearchNode<GridSearchNode>();
            AStarMax astar = new AStarMax(initialState, new GoalOnLocation(_basicWorld5X5Blocked.Goal));
            Assert.IsNotNull(astar);
            astar.Run(Int32.MaxValue);
            var maxGoal = astar.GetMaxGoal();
            Assert.AreEqual(20, maxGoal.g);
        }

        [TestMethod]
        public void Create_NewAStarGrid6X5WithBlocked0_FindPath()
        {
            GridSearchNode initialState = _basicWorld6X50.GetInitialSearchNode<GridSearchNode>();
            AStarMax astar = new AStarMax(initialState, new GoalOnLocation(_basicWorld6X50.Goal));
            Assert.IsNotNull(astar);
            astar.Run(Int32.MaxValue);
            var maxGoal = astar.GetMaxGoal();
            Assert.AreEqual(23, maxGoal.g);
        }

        [TestMethod]
        public void Create_NewAStarGrid6X5WithBlocked1_FindPath()
        {
            GridSearchNode initialState = _basicWorld6X51.GetInitialSearchNode<GridSearchNode>();
            AStarMax astar = new AStarMax(initialState, new GoalOnLocation(_basicWorld6X51.Goal));
            Assert.IsNotNull(astar);
            astar.Run(Int32.MaxValue);
            var maxGoal = astar.GetMaxGoal();
            Assert.AreEqual(22, maxGoal.g);
        }

        [TestMethod]
        public void Create_NewAStarGrid4X4WithBlocked16_FindPath()
        {
            GridSearchNode initialState = _basicWorld4X416.GetInitialSearchNode<GridSearchNode>();
            AStarMax astar = new AStarMax(initialState, new GoalOnLocation(_basicWorld4X416.Goal));
            Assert.IsNotNull(astar);
            astar.Run(Int32.MaxValue);
            var maxGoal = astar.GetMaxGoal();
            Assert.AreEqual(10, maxGoal.g);
        }

        [TestMethod]
        public void Run_AStarGrid4X4WithBlocked16_StopsByMemoryConstraint()  //TODO: fix
        {
            GridSearchNode initialState = _basicWorld4X416.GetInitialSearchNode<GridSearchNode>();
            AStarMax astar = new AStarMax(initialState, new GoalOnLocation(_basicWorld4X416.Goal));
            Assert.IsNotNull(astar);
            var howEnded = astar.Run(Int32.MaxValue, 1); //One MB memory limit
            var maxGoal = astar.GetMaxGoal();
            Assert.IsNull(maxGoal);
            Assert.AreEqual(State.StoppedByMemoryLimit, howEnded);
        }
        
    }
}
