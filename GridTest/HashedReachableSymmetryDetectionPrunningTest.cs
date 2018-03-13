using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Grid.Domain;
using MaxSearchAlg;

namespace GridTest
{
    [TestClass]
    public class HashedReachableSymmetryDetectionPrunningTest
    {

        private static World _basicClean5X5World;
        private static World _basicWorld5X5Blocked;
        private static World _basicWorld4X4;

        [TestInitialize]
        public void ClassInitialize()
        {
            _basicClean5X5World = new World(File.ReadAllText(@"..\..\Clean_Grid_5x5.grd"), new RsdUntouchedAroundTheGoalHeuristic());
            _basicWorld5X5Blocked = new World(File.ReadAllText(@"..\..\Clean_Grid_5x5BasicBlocked.grd"), new RsdUntouchedAroundTheGoalHeuristic());
            _basicWorld4X4 = new World(File.ReadAllText(@"..\..\Clean_Grid_4x4.grd"), new RsdUntouchedAroundTheGoalHeuristic());
        }
        
        [TestMethod]
        public void setAstarSolve_BasicWorld4x4_SolveCorrectly()
        {
            HashedReachableSymmetryDetectionPrunning rsd = new HashedReachableSymmetryDetectionPrunning(3);
            AStarMax solver = new AStarMax(_basicWorld4X4.GetInitialSearchNode<RsdGridSearchNode>(), rsd, new GoalOnLocation(_basicWorld4X4.Goal));
            rsd.setAstarOpenList(solver.OpenList);
            Assert.IsNotNull(rsd);
            Assert.IsNotNull(solver);
            var howEnded = solver.Run(10);
            Assert.AreEqual(State.Ended,howEnded);
            var goal = (RsdGridSearchNode)solver.GetMaxGoal();
            Assert.IsNotNull(goal);
            Assert.AreEqual(14,goal.g);
        }        

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void setAstarOpenList_CanBeCalledOnlyOnce_CallingTwiceResultsInException()
        {
            HashedReachableSymmetryDetectionPrunning rsd = new HashedReachableSymmetryDetectionPrunning(3);
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
            HashedReachableSymmetryDetectionPrunning rsd = new HashedReachableSymmetryDetectionPrunning(3);
            AStarMax solver = new AStarMax(_basicClean5X5World.GetInitialSearchNode<RsdGridSearchNode>(), rsd, new GoalOnLocation(_basicClean5X5World.Goal));
            Assert.IsNotNull(rsd);
            Assert.IsFalse(rsd.ShouldPrune(initialState));
        }

        [TestMethod]
        public void GetGrainedMap_CheckSomeMaps_ReturnsTheRightMap()
        {
            RsdGridSearchNode initialState = _basicClean5X5World.GetInitialSearchNode<RsdGridSearchNode>();
            HashedReachableSymmetryDetectionPrunning hrsd = new HashedReachableSymmetryDetectionPrunning(2);
            AStarMax solver = new AStarMax(_basicClean5X5World.GetInitialSearchNode<RsdGridSearchNode>(), hrsd, new GoalOnLocation(_basicClean5X5World.Goal));
            hrsd.setAstarOpenList(solver.OpenList);
            Assert.AreEqual(hrsd.GetVisitedGrainedMap(initialState),1);
            //Flow 1: 
            //↓→↓--
            //↓↑↓--
            //→↑*--  
            //-----
            //-----
            var Flow1Node = new RsdGridSearchNode(initialState, MoveDirection.Down);
            Assert.AreEqual(hrsd.GetVisitedGrainedMap(Flow1Node), 1);
            Flow1Node = new RsdGridSearchNode(Flow1Node, MoveDirection.Down);
            Assert.AreEqual(hrsd.GetVisitedGrainedMap(Flow1Node), 9);
            Flow1Node = new RsdGridSearchNode(Flow1Node, MoveDirection.Right);
            Assert.AreEqual(hrsd.GetVisitedGrainedMap(Flow1Node), 9);
            Flow1Node = new RsdGridSearchNode(Flow1Node, MoveDirection.Up);
            Assert.AreEqual(hrsd.GetVisitedGrainedMap(Flow1Node), 9);
            Flow1Node = new RsdGridSearchNode(Flow1Node, MoveDirection.Up);
            Assert.AreEqual(hrsd.GetVisitedGrainedMap(Flow1Node), 9);
            Flow1Node = new RsdGridSearchNode(Flow1Node, MoveDirection.Right);
            Assert.AreEqual(hrsd.GetVisitedGrainedMap(Flow1Node), 11);
            Flow1Node = new RsdGridSearchNode(Flow1Node, MoveDirection.Down);
            Assert.AreEqual(hrsd.GetVisitedGrainedMap(Flow1Node), 11);
            Flow1Node = new RsdGridSearchNode(Flow1Node, MoveDirection.Down);
            Assert.AreEqual(hrsd.GetVisitedGrainedMap(Flow1Node), 27);       
        }



        [TestMethod]
        public void PruneNode_ReturnsTrueOnRelevantStateOnly_TriggredWhenNeededByBasicSymmetryCase()
        {
            RsdGridSearchNode initialState = _basicClean5X5World.GetInitialSearchNode<RsdGridSearchNode>();
            HashedReachableSymmetryDetectionPrunning rsd = new HashedReachableSymmetryDetectionPrunning(3);
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
            HashedReachableSymmetryDetectionPrunning rsd = new HashedReachableSymmetryDetectionPrunning(2);
            AStarMax solver = new AStarMax(_basicWorld5X5Blocked.GetInitialSearchNode<RsdGridSearchNode>(), rsd, new GoalOnLocation(_basicWorld5X5Blocked.Goal));
            rsd.setAstarOpenList(solver.OpenList);
            Assert.IsFalse(rsd.ShouldPrune(initialState));
            //Flow 1: 
            //→→*
            //  #
            //###
            var Flow1Node = new RsdGridSearchNode(initialState, MoveDirection.Right);
            Assert.IsFalse(rsd.ShouldPrune(Flow1Node));
            solver.OpenList.Add(Flow1Node);
            Flow1Node = new RsdGridSearchNode(Flow1Node, MoveDirection.Right);
            Assert.IsFalse(rsd.ShouldPrune(Flow1Node));
            solver.OpenList.Add(Flow1Node);
            //Flow 2: 
            //↓→*
            //→↑#
            //###
            var Flow2Node = new RsdGridSearchNode(initialState, MoveDirection.Down);
            Assert.IsFalse(rsd.ShouldPrune(Flow2Node));
            solver.OpenList.Add(Flow2Node);
            Flow2Node = new RsdGridSearchNode(Flow2Node, MoveDirection.Right);
            Assert.IsFalse(rsd.ShouldPrune(Flow2Node));
            solver.OpenList.Add(Flow2Node);
            Flow2Node = new RsdGridSearchNode(Flow2Node, MoveDirection.Up);
            Assert.IsTrue(rsd.ShouldPrune(Flow2Node));
            //Prune = no adding here (psss... it will be added from the pruning replace)
            Flow2Node = new RsdGridSearchNode(Flow2Node, MoveDirection.Right);
            Assert.IsTrue(rsd.ShouldPrune(Flow2Node));
        }


        [TestMethod]
        public void PruneNode_ReturnsTrueOnRelevantStateOnly_TriggredWhenNeededByReachableSymmetryDetectionCase2()
        {
            //More Complicated case - prune inside the ShouldPrune method, return Yes for the should prune but replace nodes
            RsdGridSearchNode initialState = _basicWorld5X5Blocked.GetInitialSearchNode<RsdGridSearchNode>();
            HashedReachableSymmetryDetectionPrunning rsd = new HashedReachableSymmetryDetectionPrunning(3);
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
