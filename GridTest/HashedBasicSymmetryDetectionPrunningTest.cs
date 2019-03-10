using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Grid.Domain;
using MaxSearchAlg;

namespace GridTest
{
    [TestClass]
    public class HashedBasicSymmetryDetectionPrunningTest
    {

        private static World _basicClean5X5World;
        private static World _basicBlocked5X5World;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _basicClean5X5World = new World(File.ReadAllText(@"..\..\Clean_Grid_5x5.grd"), new UntouchedAroundTheGoalHeuristic());
            _basicBlocked5X5World = new World(File.ReadAllText(@"..\..\Clean_Grid_5x5BasicBlocked.grd"), new UntouchedAroundTheGoalHeuristic());
        }

        [TestMethod]
        public void PruneNode_ReturnsTrueOnRelevantStateOnly_TriggredWhenNeeded()
        {
            GridSearchNode initialState = _basicClean5X5World.GetInitialSearchNode<GridSearchNode>();
            IPrunningMethod prunningMethod = new HashedBasicSymmetryDetectionPrunning();
            Assert.IsFalse(prunningMethod.ShouldPrune(initialState));
            //Flow 1: 
            //↓→↓
            //↓↑↓
            //→↑*
            var Flow1Node = new GridSearchNode(initialState, MoveDirection.Down);
            Assert.IsFalse(prunningMethod.ShouldPrune(Flow1Node));
            Flow1Node = new GridSearchNode(Flow1Node, MoveDirection.Down);
            Assert.IsFalse(prunningMethod.ShouldPrune(Flow1Node));
            Flow1Node = new GridSearchNode(Flow1Node, MoveDirection.Right);
            Assert.IsFalse(prunningMethod.ShouldPrune(Flow1Node));
            Flow1Node = new GridSearchNode(Flow1Node, MoveDirection.Up);
            Assert.IsFalse(prunningMethod.ShouldPrune(Flow1Node));
            Flow1Node = new GridSearchNode(Flow1Node, MoveDirection.Up);
            Assert.IsFalse(prunningMethod.ShouldPrune(Flow1Node));
            Flow1Node = new GridSearchNode(Flow1Node, MoveDirection.Right);
            Assert.IsFalse(prunningMethod.ShouldPrune(Flow1Node));
            Flow1Node = new GridSearchNode(Flow1Node, MoveDirection.Down);
            Assert.IsFalse(prunningMethod.ShouldPrune(Flow1Node));
            Flow1Node = new GridSearchNode(Flow1Node, MoveDirection.Down);
            Assert.IsFalse(prunningMethod.ShouldPrune(Flow1Node));
            //Flow 2: 
            //→→↓
            //↓←←
            //→→*
            var Flow2Node = new GridSearchNode(initialState, MoveDirection.Right);
            Assert.IsFalse(prunningMethod.ShouldPrune(Flow2Node));
            Flow2Node = new GridSearchNode(Flow2Node, MoveDirection.Right);
            Assert.IsFalse(prunningMethod.ShouldPrune(Flow2Node));
            Flow2Node = new GridSearchNode(Flow2Node, MoveDirection.Down);
            Assert.IsFalse(prunningMethod.ShouldPrune(Flow2Node));
            Flow2Node = new GridSearchNode(Flow2Node, MoveDirection.Left);
            Assert.IsFalse(prunningMethod.ShouldPrune(Flow2Node));
            Flow2Node = new GridSearchNode(Flow2Node, MoveDirection.Left);
            Assert.IsFalse(prunningMethod.ShouldPrune(Flow2Node));
            Flow2Node = new GridSearchNode(Flow2Node, MoveDirection.Down);
            Assert.IsFalse(prunningMethod.ShouldPrune(Flow2Node));
            Flow2Node = new GridSearchNode(Flow2Node, MoveDirection.Right);
            Assert.IsFalse(prunningMethod.ShouldPrune(Flow2Node));
            Flow2Node = new GridSearchNode(Flow2Node, MoveDirection.Right);
            Assert.IsTrue(prunningMethod.ShouldPrune(Flow2Node));
        }


        [TestMethod]
        public void Basic_Hbsd_findPathDfBnB()
        {
            GridSearchNode initialState = _basicBlocked5X5World.GetInitialSearchNode<GridSearchNode>();
            IPrunningMethod prunningMethod = new HashedBasicSymmetryDetectionPrunning();
            Solver solver = new DfBnbMax(initialState, prunningMethod,  new GoalOnLocation(_basicBlocked5X5World.Goal));
            Assert.IsNotNull(solver);
            solver.Run(Int32.MaxValue); //Prevent stoping by time, should stop only when goal found
            var maxGoal = solver.GetMaxGoal();
            Assert.AreEqual(20, maxGoal.g);
        }

        [TestMethod]
        public void Basic_Hbsd_findPathAstar()
        {
            GridSearchNode initialState = _basicBlocked5X5World.GetInitialSearchNode<GridSearchNode>();
            IPrunningMethod prunningMethod = new HashedBasicSymmetryDetectionPrunning();
            Solver solver = new AStarMax(initialState, prunningMethod,  new GoalOnLocation(_basicBlocked5X5World.Goal));
            Assert.IsNotNull(solver);
            solver.Run(Int32.MaxValue); //Prevent stoping by time, should stop only when goal found
            var maxGoal = solver.GetMaxGoal();
            Assert.AreEqual(20, maxGoal.g);
        }
    }
}
