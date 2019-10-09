using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Grid.Domain;
using MaxSearchAlg;

namespace GridTest
{
    [TestClass]
    public class BiconnectedComponentsHeuristicTest
    {
        private static World _basicWorldV1;
        private static World _basicWorldV1unTouched;
        private static World _basicWorldT1S0;
        private static World _basicWorldBccAndPruningCheck;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _basicWorldV1 = new World(File.ReadAllText(@"..\..\Grid_5x5BiconnectedComponentsHeuristicV1.grd"), 
                new BiconnectedComponentsHeuristic(), WorldType.Uniform);
            _basicWorldV1unTouched = new World(File.ReadAllText(@"..\..\Grid_5x5BCCHV1asReachable.grd"), 
                new UntouchedAroundTheGoalHeuristic(), WorldType.Uniform);
            _basicWorldT1S0 = new World(File.ReadAllText(@"..\..\Grid_5x5BccH_try1step0.grd"),
                new BiconnectedComponentsHeuristic(), WorldType.Uniform);
            _basicWorldBccAndPruningCheck = new World(File.ReadAllText(@"..\..\Clean_Grid_5x5BccAndPruning.grd"),
                new BiconnectedComponentsHeuristic(), WorldType.Uniform);
        }

        [TestMethod]
        public void gValue_getFvalueForChildNode_Equals24()
        {
            GridSearchNode initialStateBCC = _basicWorldV1.GetInitialSearchNode<GridSearchNode>();
            GridSearchNode initialStateUntouched = _basicWorldV1unTouched.GetInitialSearchNode<GridSearchNode>();
            Assert.AreEqual(initialStateUntouched.h, initialStateBCC.h);
        }

        [TestMethod]
        public void AStarGrid_BasicRunWithBCCHeuristics_Success()
        {
            GridSearchNode initialState = _basicWorldV1.GetInitialSearchNode<GridSearchNode>();
            AStarMax astar = new AStarMax(initialState, new GoalOnLocation(_basicWorldV1.Goal));
            Assert.IsNotNull(astar);
            var howEnded = astar.Run(Int32.MaxValue); 
            var maxGoal = astar.GetMaxGoal();
            Assert.IsNotNull(maxGoal);
            Assert.AreEqual(State.Ended, howEnded);
        }

        [TestMethod]
        public void AStarGrid_BasicRun_BCCHeuristics_Success()
        {
            GridSearchNode initialState = _basicWorldT1S0.GetInitialSearchNode<GridSearchNode>();
            AStarMax astar = new AStarMax(initialState, new GoalOnLocation(_basicWorldV1.Goal));
            Assert.IsNotNull(astar);
            var howEnded = astar.Run(Int32.MaxValue); 
            var maxGoal = astar.GetMaxGoal();
            Assert.IsNotNull(maxGoal);
            Assert.AreEqual(State.Ended, howEnded);
        }


        [TestMethod]
        public void AStarGrid_BasicRun_BCCHeuristicsBSD_Success()
        {
            GridSearchNode initialState = _basicWorldT1S0.GetInitialSearchNode<GridSearchNode>();
            AStarMax astar = new AStarMax(initialState, new BasicSymmetryDetectionPrunning(), new GoalOnLocation(_basicWorldV1.Goal));
            Assert.IsNotNull(astar);
            var howEnded = astar.Run(Int32.MaxValue);
            var maxGoal = astar.GetMaxGoal();
            Assert.IsNotNull(maxGoal);
            Assert.AreEqual(State.Ended, howEnded);
        }

        [TestMethod]
        public void AStarGrid_BasicRun_BCCHeuristicsBSD_Success2()
        {
            GridSearchNode initialState = _basicWorldBccAndPruningCheck.GetInitialSearchNode<GridSearchNode>();
            AStarMax astar = new AStarMax(initialState, new BasicSymmetryDetectionPrunning(), new GoalOnLocation(_basicWorldV1.Goal));
            Assert.IsNotNull(astar);
            var howEnded = astar.Run(Int32.MaxValue);
            var maxGoal = astar.GetMaxGoal();
            Assert.IsNotNull(maxGoal);
            Assert.AreEqual(State.Ended, howEnded);
        }


        [TestMethod]
        public void AStarGrid_BasicRun_BCCHeuristicsHBSD_Success()
        {
            GridSearchNode initialState = _basicWorldT1S0.GetInitialSearchNode<GridSearchNode>();
            AStarMax astar = new AStarMax(initialState, new HashedBasicSymmetryDetectionPrunning(), new GoalOnLocation(_basicWorldV1.Goal));
            Assert.IsNotNull(astar);
            var howEnded = astar.Run(Int32.MaxValue);
            var maxGoal = astar.GetMaxGoal();
            Assert.IsNotNull(maxGoal);
            Assert.AreEqual(State.Ended, howEnded);
        }

        [TestMethod]
        public void AStarGrid_BasicRun_BCCHeuristicsHBSD_Success2()
        {
            GridSearchNode initialState = _basicWorldBccAndPruningCheck.GetInitialSearchNode<GridSearchNode>();
            AStarMax astar = new AStarMax(initialState, new HashedBasicSymmetryDetectionPrunning(), new GoalOnLocation(_basicWorldV1.Goal));
            Assert.IsNotNull(astar);
            var howEnded = astar.Run(Int32.MaxValue);
            var maxGoal = astar.GetMaxGoal();
            Assert.IsNotNull(maxGoal);
            Assert.AreEqual(State.Ended, howEnded);
        }




        [TestMethod]
        public void AStarGrid_BasicRun_BCCHeuristicsRSD_Success()
        {
            RsdGridSearchNode initialState = _basicWorldT1S0.GetInitialSearchNode<RsdGridSearchNode>();
            ReachableSymmetryDetectionPrunning pruning  = new ReachableSymmetryDetectionPrunning();
            AStarMax astar = new AStarMax(initialState, pruning, new GoalOnLocation(_basicWorldV1.Goal));
            Assert.IsNotNull(astar);
            pruning.setAstarOpenList(astar.OpenList);
            var howEnded = astar.Run(Int32.MaxValue);
            var maxGoal = astar.GetMaxGoal();
            Assert.IsNotNull(maxGoal);
            Assert.AreEqual(State.Ended, howEnded);
        }

        [TestMethod]
        public void AStarGrid_BasicRun_BCCHeuristicsRSD_Success2()
        {
            RsdGridSearchNode initialState = _basicWorldBccAndPruningCheck.GetInitialSearchNode<RsdGridSearchNode>();
            ReachableSymmetryDetectionPrunning pruning  = new ReachableSymmetryDetectionPrunning();
            AStarMax astar = new AStarMax(initialState, pruning, new GoalOnLocation(_basicWorldV1.Goal));
            Assert.IsNotNull(astar);
            pruning.setAstarOpenList(astar.OpenList);
            var howEnded = astar.Run(Int32.MaxValue);
            var maxGoal = astar.GetMaxGoal();
            Assert.IsNotNull(maxGoal);
            Assert.AreEqual(State.Ended, howEnded);
        }


    }
}
