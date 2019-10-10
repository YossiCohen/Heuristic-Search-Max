
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
        private static World _basicWorld3X3Life;
        private static World _basicWorld5X5Blocked;
        private static World _basicWorld5X5BlockedLife;
        private static World _basicWorld6X50;
        private static World _basicWorld6X51;
        private static World _basicWorld6X51Life;
        private static World _basicWorld4X416;
        private static World _basicWorld5X5ThreeBlockedLife;
        private static World _basicWorld7x6MiddleWallLife;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _basicWorld3X3 = new World(File.ReadAllText(@"..\..\33.grd"), new UntouchedAroundTheGoalHeuristic(), WorldType.Uniform);
            _basicWorld3X3Life = new World(File.ReadAllText(@"..\..\33.grd"), new UntouchedAroundTheGoalHeuristic(), WorldType.Life);
            _basicWorld5X5Blocked = new World(File.ReadAllText(@"..\..\Clean_Grid_5x5BasicBlocked.grd"), new UntouchedAroundTheGoalHeuristic(), WorldType.Uniform);
            _basicWorld5X5BlockedLife = new World(File.ReadAllText(@"..\..\Clean_Grid_5x5BasicBlocked.grd"), new UntouchedAroundTheGoalHeuristic(), WorldType.Life);
            _basicWorld6X50 = new World(File.ReadAllText(@"..\..\Grid-10-6-5-4-0.grd"), new UntouchedAroundTheGoalHeuristic(), WorldType.Uniform);
            _basicWorld6X51 = new World(File.ReadAllText(@"..\..\Grid-10-6-5-4-1.grd"), new UntouchedAroundTheGoalHeuristic(), WorldType.Uniform);
            _basicWorld6X51Life = new World(File.ReadAllText(@"..\..\Grid-10-6-5-4-1.grd"), new UntouchedAroundTheGoalHeuristic(), WorldType.Life);
            _basicWorld4X416 = new World(File.ReadAllText(@"..\..\Grid-20-4-4-2-16.grd"), new UntouchedAroundTheGoalHeuristic(), WorldType.Uniform);
            _basicWorld5X5ThreeBlockedLife = new World(File.ReadAllText(@"..\..\AlternateStepsHeuristic_Test_5x5_B3_E1_O2.grd"), new UntouchedAroundTheGoalHeuristic(), WorldType.Life);
            _basicWorld7x6MiddleWallLife = new World(File.ReadAllText(@"..\..\AlternateStepsHeuristic_Test_7x6v1.grd"), new UntouchedAroundTheGoalHeuristic(), WorldType.Life);
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
        public void Create_NewAStarGrid3X3_findPathLife()
        {
            GridSearchNode initialState = _basicWorld3X3Life.GetInitialSearchNode<GridSearchNode>();
            AStarMax astar = new AStarMax(initialState, new GoalOnLocation(_basicWorld3X3.Goal));
            Assert.IsNotNull(astar);
            astar.Run(Int32.MaxValue); //Prevent stoping by time, should stop only when goal found
            var maxGoal = astar.GetMaxGoal();
            Assert.AreEqual(17, maxGoal.g);
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
        public void Create_NewAStarGrid5X5WithBlocked_FindPathLife()
        {
            GridSearchNode initialState = _basicWorld5X5BlockedLife.GetInitialSearchNode<GridSearchNode>();
            AStarMax astar = new AStarMax(initialState, new GoalOnLocation(_basicWorld5X5Blocked.Goal));
            Assert.IsNotNull(astar);
            astar.Run(Int32.MaxValue);
            var maxGoal = astar.GetMaxGoal();
            Assert.AreEqual(63, maxGoal.g);
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
        public void Create_NewAStarGrid6X5WithBlocked1_FindPathLife()
        {
            GridSearchNode initialState = _basicWorld6X51Life.GetInitialSearchNode<GridSearchNode>();
            AStarMax astar = new AStarMax(initialState, new GoalOnLocation(_basicWorld6X51.Goal));
            Assert.IsNotNull(astar);
            astar.Run(Int32.MaxValue);
            var maxGoal = astar.GetMaxGoal();
            Assert.AreEqual(74, maxGoal.g);
        }

        [TestMethod]
        public void Create_NewAStarGrid5X5ThreeBlocked_FindPathLife()
        {
            GridSearchNode initialState = _basicWorld5X5ThreeBlockedLife.GetInitialSearchNode<GridSearchNode>();
            AStarMax astar = new AStarMax(initialState, new GoalOnLocation(_basicWorld5X5ThreeBlockedLife.Goal));
            Assert.IsNotNull(astar);
            astar.Run(Int32.MaxValue);
            var maxGoal = astar.GetMaxGoal();
            Assert.AreEqual(63, maxGoal.g);
        }

        [TestMethod]
        public void Create_NewAStarGrid7X6MiddleWallLife_FindPathLife()
        {
            GridSearchNode initialState = _basicWorld7x6MiddleWallLife.GetInitialSearchNode<GridSearchNode>();
            AStarMax astar = new AStarMax(initialState, new GoalOnLocation(_basicWorld7x6MiddleWallLife.Goal));
            Assert.IsNotNull(astar);
            astar.Run(Int32.MaxValue);
            var maxGoal = astar.GetMaxGoal();
            Assert.AreEqual(112, maxGoal.g);
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
        public void Run_AStarGrid4X4WithBlocked16_StopsByMemoryConstraint()
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
