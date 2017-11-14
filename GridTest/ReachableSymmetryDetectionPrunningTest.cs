using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Grid.Domain;
using MaxSearchAlg;

namespace GridTest
{
    [TestClass]
    public class ReachableSymmetryDetectionPrunningTest
    {

        private static World _basicClean5X5World;
        private static World _basicWorld5X5Blocked;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _basicClean5X5World = new World(File.ReadAllText(@"..\..\Clean_Grid_5x5.grd"), new RsdUntouchedAroundTheGoalHeuristic());
            _basicWorld5X5Blocked = new World(File.ReadAllText(@"..\..\Clean_Grid_5x5BasicBlocked.grd"), new UntouchedAroundTheGoalHeuristic());
        }

        [TestMethod]
        public void PruneNode_ReturnsTrueOnRelevantStateOnly_TriggredWhenNeededByBasicSymmetryCase()
        {
            RsdGridSearchNode initialState = _basicClean5X5World.GetInitialSearchNode<RsdGridSearchNode>();
            IPrunningMethod prunningMethod = new ReachableSymmetryDetectionPrunning();
            Assert.IsFalse(prunningMethod.ShouldPrune(initialState));
            //Flow 1: 
            //↓→↓
            //↓↑↓
            //→↑*
            var Flow1Node = new RsdGridSearchNode(initialState, MoveDirection.Down);
            Assert.IsFalse(prunningMethod.ShouldPrune(Flow1Node));
            Flow1Node = new RsdGridSearchNode(Flow1Node, MoveDirection.Down);
            Assert.IsFalse(prunningMethod.ShouldPrune(Flow1Node));
            Flow1Node = new RsdGridSearchNode(Flow1Node, MoveDirection.Right);
            Assert.IsFalse(prunningMethod.ShouldPrune(Flow1Node));
            Flow1Node = new RsdGridSearchNode(Flow1Node, MoveDirection.Up);
            Assert.IsFalse(prunningMethod.ShouldPrune(Flow1Node));
            Flow1Node = new RsdGridSearchNode(Flow1Node, MoveDirection.Up);
            Assert.IsFalse(prunningMethod.ShouldPrune(Flow1Node));
            Flow1Node = new RsdGridSearchNode(Flow1Node, MoveDirection.Right);
            Assert.IsFalse(prunningMethod.ShouldPrune(Flow1Node));
            Flow1Node = new RsdGridSearchNode(Flow1Node, MoveDirection.Down);
            Assert.IsFalse(prunningMethod.ShouldPrune(Flow1Node));
            Flow1Node = new RsdGridSearchNode(Flow1Node, MoveDirection.Down);
            Assert.IsFalse(prunningMethod.ShouldPrune(Flow1Node));
            //Flow 2: 
            //→→↓
            //↓←←
            //→→*
            var Flow2Node = new RsdGridSearchNode(initialState, MoveDirection.Right);
            Assert.IsFalse(prunningMethod.ShouldPrune(Flow2Node));
            Flow2Node = new RsdGridSearchNode(Flow2Node, MoveDirection.Right);
            Assert.IsFalse(prunningMethod.ShouldPrune(Flow2Node));
            Flow2Node = new RsdGridSearchNode(Flow2Node, MoveDirection.Down);
            Assert.IsFalse(prunningMethod.ShouldPrune(Flow2Node));
            Flow2Node = new RsdGridSearchNode(Flow2Node, MoveDirection.Left);
            Assert.IsFalse(prunningMethod.ShouldPrune(Flow2Node));
            Flow2Node = new RsdGridSearchNode(Flow2Node, MoveDirection.Left);
            Assert.IsFalse(prunningMethod.ShouldPrune(Flow2Node));
            Flow2Node = new RsdGridSearchNode(Flow2Node, MoveDirection.Down);
            Assert.IsFalse(prunningMethod.ShouldPrune(Flow2Node));
            Flow2Node = new RsdGridSearchNode(Flow2Node, MoveDirection.Right);
            Assert.IsFalse(prunningMethod.ShouldPrune(Flow2Node));
            Flow2Node = new RsdGridSearchNode(Flow2Node, MoveDirection.Right);
            Assert.IsTrue(prunningMethod.ShouldPrune(Flow2Node));
        }



        [TestMethod]
        public void PruneNode_ReturnsTrueOnRelevantStateOnly_TriggredWhenNeededByReachableSymmetryDetectionCase1()
        {
            RsdGridSearchNode initialState = _basicClean5X5World.GetInitialSearchNode<RsdGridSearchNode>();
            IPrunningMethod prunningMethod = new ReachableSymmetryDetectionPrunning();
            Assert.IsFalse(prunningMethod.ShouldPrune(initialState));
            //Flow 1: 
            //→→*
            //  #
            //###
            var Flow1Node = new RsdGridSearchNode(initialState, MoveDirection.Right);
            Assert.IsFalse(prunningMethod.ShouldPrune(Flow1Node));
            Flow1Node = new RsdGridSearchNode(Flow1Node, MoveDirection.Right);
            Assert.IsFalse(prunningMethod.ShouldPrune(Flow1Node));
            //Flow 2: 
            //↓→*
            //→↑#
            //###
            var Flow2Node = new RsdGridSearchNode(initialState, MoveDirection.Down);
            Assert.IsFalse(prunningMethod.ShouldPrune(Flow2Node));
            Flow2Node = new RsdGridSearchNode(Flow2Node, MoveDirection.Right);
            Assert.IsFalse(prunningMethod.ShouldPrune(Flow2Node));
            Flow2Node = new RsdGridSearchNode(Flow2Node, MoveDirection.Up);
            Assert.IsFalse(prunningMethod.ShouldPrune(Flow2Node));
            Flow2Node = new RsdGridSearchNode(Flow2Node, MoveDirection.Right);
            Assert.IsTrue(prunningMethod.ShouldPrune(Flow2Node));

        }
    }
}
