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

        [TestInitialize]
        public void ClassInitialize()
        {
            _basicClean5X5World = new World(File.ReadAllText(@"..\..\Clean_Grid_5x5.grd"), new RsdUntouchedAroundTheGoalHeuristic());
            _basicWorld5X5Blocked = new World(File.ReadAllText(@"..\..\Clean_Grid_5x5BasicBlocked.grd"), new RsdUntouchedAroundTheGoalHeuristic());
        }
        
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void setAstarOpenList_CanBeCalledOnlyOnce_CallingTwiceResultsInException()
        {
            ReachableSymmetryDetectionPrunning rsd = new ReachableSymmetryDetectionPrunning();
            AStarMax solver = new AStarMax(_basicClean5X5World.GetInitialSearchNode<RsdGridSearchNode>(), rsd, new GoalOnLocation(_basicClean5X5World.Goal));
            Assert.IsNotNull(rsd);
            rsd.setAstarOpenList(solver.OpenList);
            rsd.setAstarOpenList(solver.OpenList);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void IsPrune_NotWorkingWithoutAstarOpenList_Exception()
        {
            RsdGridSearchNode initialState = _basicClean5X5World.GetInitialSearchNode<RsdGridSearchNode>();
            ReachableSymmetryDetectionPrunning rsd = new ReachableSymmetryDetectionPrunning();
            AStarMax solver = new AStarMax(_basicClean5X5World.GetInitialSearchNode<RsdGridSearchNode>(), rsd, new GoalOnLocation(_basicClean5X5World.Goal));
            Assert.IsNotNull(rsd);
            Assert.IsFalse(rsd.ShouldPrune(initialState));
        }

        [TestMethod]
        public void PruneNode_ReturnsTrueOnRelevantStateOnly_TriggredWhenNeededByBasicSymmetryCase()
        {
            RsdGridSearchNode initialState = _basicClean5X5World.GetInitialSearchNode<RsdGridSearchNode>();
            ReachableSymmetryDetectionPrunning rsd = new ReachableSymmetryDetectionPrunning();
            AStarMax solver = new AStarMax(_basicClean5X5World.GetInitialSearchNode<RsdGridSearchNode>(), rsd, new GoalOnLocation(_basicClean5X5World.Goal));
            rsd.setAstarOpenList(solver.OpenList);
            Assert.IsFalse(rsd.ShouldPrune(initialState));
            //Flow 1: 
            //↓→↓
            //↓↑↓
            //→↑*
            var Flow1Node = new RsdGridSearchNode(initialState, MoveDirection.Down);
            Assert.IsFalse(rsd.ShouldPrune(Flow1Node));
            Flow1Node = new RsdGridSearchNode(Flow1Node, MoveDirection.Down);
            Assert.IsFalse(rsd.ShouldPrune(Flow1Node));
            Flow1Node = new RsdGridSearchNode(Flow1Node, MoveDirection.Right);
            Assert.IsFalse(rsd.ShouldPrune(Flow1Node));
            Flow1Node = new RsdGridSearchNode(Flow1Node, MoveDirection.Up);
            Assert.IsFalse(rsd.ShouldPrune(Flow1Node));
            Flow1Node = new RsdGridSearchNode(Flow1Node, MoveDirection.Up);
            Assert.IsFalse(rsd.ShouldPrune(Flow1Node));
            Flow1Node = new RsdGridSearchNode(Flow1Node, MoveDirection.Right);
            Assert.IsFalse(rsd.ShouldPrune(Flow1Node));
            Flow1Node = new RsdGridSearchNode(Flow1Node, MoveDirection.Down);
            Assert.IsFalse(rsd.ShouldPrune(Flow1Node));
            Flow1Node = new RsdGridSearchNode(Flow1Node, MoveDirection.Down);
            Assert.IsFalse(rsd.ShouldPrune(Flow1Node));
            //Flow 2: 
            //→→↓
            //↓←←
            //→→*
            var Flow2Node = new RsdGridSearchNode(initialState, MoveDirection.Right);
            Assert.IsFalse(rsd.ShouldPrune(Flow2Node));
            Flow2Node = new RsdGridSearchNode(Flow2Node, MoveDirection.Right);
            Assert.IsFalse(rsd.ShouldPrune(Flow2Node));
            Flow2Node = new RsdGridSearchNode(Flow2Node, MoveDirection.Down);
            Assert.IsFalse(rsd.ShouldPrune(Flow2Node));
            Flow2Node = new RsdGridSearchNode(Flow2Node, MoveDirection.Left);
            Assert.IsFalse(rsd.ShouldPrune(Flow2Node));
            Flow2Node = new RsdGridSearchNode(Flow2Node, MoveDirection.Left);
            Assert.IsFalse(rsd.ShouldPrune(Flow2Node));
            Flow2Node = new RsdGridSearchNode(Flow2Node, MoveDirection.Down);
            Assert.IsFalse(rsd.ShouldPrune(Flow2Node));
            Flow2Node = new RsdGridSearchNode(Flow2Node, MoveDirection.Right);
            Assert.IsFalse(rsd.ShouldPrune(Flow2Node));
            Flow2Node = new RsdGridSearchNode(Flow2Node, MoveDirection.Right);
            Assert.IsTrue(rsd.ShouldPrune(Flow2Node));
        }



        [TestMethod]
        public void PruneNode_ReturnsTrueOnRelevantStateOnly_TriggredWhenNeededByReachableSymmetryDetectionCase1()
        {
            RsdGridSearchNode initialState = _basicWorld5X5Blocked.GetInitialSearchNode<RsdGridSearchNode>();
            ReachableSymmetryDetectionPrunning rsd = new ReachableSymmetryDetectionPrunning();
            AStarMax solver = new AStarMax(_basicWorld5X5Blocked.GetInitialSearchNode<RsdGridSearchNode>(), rsd, new GoalOnLocation(_basicWorld5X5Blocked.Goal));
            rsd.setAstarOpenList(solver.OpenList);
            Assert.IsFalse(rsd.ShouldPrune(initialState));
            //Flow 1: 
            //→→*
            //  #
            //###
            var Flow1Node = new RsdGridSearchNode(initialState, MoveDirection.Right);
            Assert.IsFalse(rsd.ShouldPrune(Flow1Node));
            Flow1Node = new RsdGridSearchNode(Flow1Node, MoveDirection.Right);
            Assert.IsFalse(rsd.ShouldPrune(Flow1Node));
            //Flow 2: 
            //↓→*
            //→↑#
            //###
            var Flow2Node = new RsdGridSearchNode(initialState, MoveDirection.Down);
            Assert.IsFalse(rsd.ShouldPrune(Flow2Node));
            Flow2Node = new RsdGridSearchNode(Flow2Node, MoveDirection.Right);
            Assert.IsFalse(rsd.ShouldPrune(Flow2Node));
            Flow2Node = new RsdGridSearchNode(Flow2Node, MoveDirection.Up);
            Assert.IsTrue(rsd.ShouldPrune(Flow2Node));
            Flow2Node = new RsdGridSearchNode(Flow2Node, MoveDirection.Right);
            Assert.IsTrue(rsd.ShouldPrune(Flow2Node));
        }


        [TestMethod]
        public void PruneNode_ReturnsTrueOnRelevantStateOnly_TriggredWhenNeededByReachableSymmetryDetectionCase2()
        {
            //More Complicated case - prune inside the ShouldPrune method, return Yes for the should prune but replace nodes
            RsdGridSearchNode initialState = _basicWorld5X5Blocked.GetInitialSearchNode<RsdGridSearchNode>();
            ReachableSymmetryDetectionPrunning rsd = new ReachableSymmetryDetectionPrunning();
            AStarMax solver = new AStarMax(_basicWorld5X5Blocked.GetInitialSearchNode<RsdGridSearchNode>(), rsd, new GoalOnLocation(_basicWorld5X5Blocked.Goal));
            rsd.setAstarOpenList(solver.OpenList);
            Assert.IsFalse(rsd.ShouldPrune(initialState));
            //Flow 1: 
            //↓→*
            //→↑#
            //###
            var Flow2Node = new RsdGridSearchNode(initialState, MoveDirection.Down);
            Assert.IsFalse(rsd.ShouldPrune(Flow2Node));
            Flow2Node = new RsdGridSearchNode(Flow2Node, MoveDirection.Right);
            Assert.IsFalse(rsd.ShouldPrune(Flow2Node));
            Flow2Node = new RsdGridSearchNode(Flow2Node, MoveDirection.Up);
            Assert.IsFalse(rsd.ShouldPrune(Flow2Node));
            Flow2Node = new RsdGridSearchNode(Flow2Node, MoveDirection.Right);
            Assert.IsFalse(rsd.ShouldPrune(Flow2Node));
            //Flow 2: 
            //→→*
            //  #
            //###
            var Flow1Node = new RsdGridSearchNode(initialState, MoveDirection.Right);
            Assert.IsTrue(rsd.ShouldPrune(Flow1Node));
            Flow1Node = new RsdGridSearchNode(Flow1Node, MoveDirection.Right);
            Assert.IsTrue(rsd.ShouldPrune(Flow1Node));
        }
    }
}
