
using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Grid.Domain;
using MaxSearchAlg;

namespace GridTest
{
    [TestClass]
    public class GridGreedyMaxTest
    {
        private static World _basicWorld3X3;
        private static World _basicWorld5X5Blocked;
        private static World _basicWorld6X50;
        private static World _basicWorld6X51;
        private static World _basicWorld4X416;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _basicWorld3X3 = new World(File.ReadAllText(@"..\..\33.grd"), new SeparateAlternateStepsBiconnectedComponentsHeuristic());
            _basicWorld5X5Blocked = new World(File.ReadAllText(@"..\..\Clean_Grid_5x5BasicBlocked.grd"), new SeparateAlternateStepsBiconnectedComponentsHeuristic());
            _basicWorld6X50 = new World(File.ReadAllText(@"..\..\Grid-10-6-5-4-0.grd"), new SeparateAlternateStepsBiconnectedComponentsHeuristic());
            _basicWorld6X51 = new World(File.ReadAllText(@"..\..\Grid-10-6-5-4-1.grd"), new SeparateAlternateStepsBiconnectedComponentsHeuristic());
            _basicWorld4X416 = new World(File.ReadAllText(@"..\..\Grid-20-4-4-2-16.grd"), new SeparateAlternateStepsBiconnectedComponentsHeuristic());
        }

        [TestMethod]
        public void Create_NewGrid3X3_findGreedyPath()
        {
            GridSearchNode initialState = _basicWorld3X3.GetInitialSearchNode<GridSearchNode>();
            GreedyMax greedy = new GreedyMax(initialState, new GoalOnLocation(_basicWorld3X3.Goal));
            Assert.IsNotNull(greedy);
            greedy.Run(Int32.MaxValue); //Prevent stoping by time, should stop only when goal found
            var maxGoal = greedy.GetMaxGoal();
            Assert.AreEqual(8, maxGoal.g);
        }

        [TestMethod]
        public void Create_NewGrid5X5WithBlocked_FindGreedyPath()
        {
            GridSearchNode initialState = _basicWorld5X5Blocked.GetInitialSearchNode<GridSearchNode>();
            GreedyMax greedy = new GreedyMax(initialState, new GoalOnLocation(_basicWorld5X5Blocked.Goal));
            Assert.IsNotNull(greedy);
            greedy.Run(Int32.MaxValue);
            var maxGoal = greedy.GetMaxGoal();
            Assert.AreEqual(20, maxGoal.g);
        }

        [TestMethod]
        public void Create_NewGrid6X5WithBlocked0_FindGreedyPath()
        {
            GridSearchNode initialState = _basicWorld6X50.GetInitialSearchNode<GridSearchNode>();
            GreedyMax greedy = new GreedyMax(initialState, new GoalOnLocation(_basicWorld6X50.Goal));
            Assert.IsNotNull(greedy);
            greedy.Run(Int32.MaxValue);
            var maxGoal = greedy.GetMaxGoal();
            Assert.AreEqual(23, maxGoal.g);
        }

        [TestMethod]
        public void Create_NewGrid6X5WithBlocked1_FindGreedyPath()
        {
            GridSearchNode initialState = _basicWorld6X51.GetInitialSearchNode<GridSearchNode>();
            GreedyMax greedy = new GreedyMax(initialState, new GoalOnLocation(_basicWorld6X51.Goal));
            Assert.IsNotNull(greedy);
            greedy.Run(Int32.MaxValue);
            var maxGoal = greedy.GetMaxGoal();
            Assert.AreEqual(22, maxGoal.g);
        }

        [TestMethod]
        public void Create_NewGrid4X4WithBlocked16_FindGreedyPath()
        {
            GridSearchNode initialState = _basicWorld4X416.GetInitialSearchNode<GridSearchNode>();
            GreedyMax greedy = new GreedyMax(initialState, new GoalOnLocation(_basicWorld4X416.Goal));
            Assert.IsNotNull(greedy);
            greedy.Run(Int32.MaxValue);
            var maxGoal = greedy.GetMaxGoal();
            Assert.AreEqual(10, maxGoal.g);
        }

        [TestMethod]
        public void Run_Grid4X4WithBlocked16_StopsByMemoryConstraint()  //TODO:fix
        {
            GridSearchNode initialState = _basicWorld4X416.GetInitialSearchNode<GridSearchNode>();
            GreedyMax greedy = new GreedyMax(initialState, new GoalOnLocation(_basicWorld4X416.Goal));
            Assert.IsNotNull(greedy);
            var howEnded = greedy.Run(Int32.MaxValue, 1); //One MB memory limit
            var maxGoal = greedy.GetMaxGoal();
            Assert.IsNull(maxGoal);
            Assert.AreEqual(State.StoppedByMemoryLimit, howEnded);
        }
        
    }
}
