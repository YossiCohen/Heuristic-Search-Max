
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Grid.Domain;

namespace GridTest
{
    [TestClass]
    public class GridSearchNodeTest
    {
        private static World _basicWorld;
        private static World _basicWorld3X3;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _basicWorld3X3 = new World(File.ReadAllText(@"..\..\33.grd"), new UntouchedAroundTheGoalHeuristic(), WorldType.Uniform);
            _basicWorld = new World(File.ReadAllText(@"..\..\Grid-10-6-5-4-0.grd"), new UntouchedAroundTheGoalHeuristic(), WorldType.Uniform);
        }

        [TestMethod]
        public void Constructor_CreatesState_Created()
        {
            GridSearchNode gn = new GridSearchNode(_basicWorld);
            Assert.IsNotNull(gn);
        }

        [TestMethod]
        public void Constructor_CreatesStateWithParent_Created()
        {
            GridSearchNode parent = _basicWorld.GetInitialSearchNode<GridSearchNode>();
            GridSearchNode gn = new GridSearchNode(parent, MoveDirection.Down);
            Assert.IsNotNull(gn);
        }

        [TestMethod]
        public void fValue_getFvalueForNewNode_EqualsZero()
        {
            GridSearchNode parent = _basicWorld.GetInitialSearchNode<GridSearchNode>();
            Assert.AreEqual(0, parent.g);
        }

        [TestMethod]
        public void fValue_getFvalueForChildNode_Equals1()
        {
            GridSearchNode parent = _basicWorld.GetInitialSearchNode<GridSearchNode>();
            GridSearchNode gn = new GridSearchNode(parent, MoveDirection.Down);
            Assert.AreEqual(1, gn.g);
        }

        [TestMethod]
        public void Children_GoalHaveNoChildren_CountEquals0()
        {
            GridSearchNode parent = _basicWorld3X3.GetInitialSearchNode<GridSearchNode>();
            //GoingToTheGoalPosition
            var gridNode = new GridSearchNode(parent, MoveDirection.Down);
            gridNode = new GridSearchNode(gridNode, MoveDirection.Down);
            gridNode = new GridSearchNode(gridNode, MoveDirection.Right);
            gridNode = new GridSearchNode(gridNode, MoveDirection.Right);
            Assert.AreEqual(0, gridNode.Children.Count);
        }

        [TestMethod]
        public void Children_AreAllwaysNearThierParents_DeltaDistanceAlways1()
        {
            GridSearchNode parent = _basicWorld3X3.GetInitialSearchNode<GridSearchNode>();
            //GoingToTheGoalPosition
            var gridNode = new GridSearchNode(parent, MoveDirection.Down);
            foreach (var NodeChild in gridNode.Children)
            {
                var gridNodeChild = NodeChild as GridSearchNode;
                Assert.AreEqual(1, Math.Abs(gridNode.HeadLocation.X - gridNodeChild.HeadLocation.X) + Math.Abs(gridNode.HeadLocation.Y - gridNodeChild.HeadLocation.Y));
            }
            var gridNode2 = new GridSearchNode(gridNode, MoveDirection.Right);
            foreach (var nodeChild in gridNode2.Children)
            {
                var gridNodeChild = nodeChild as GridSearchNode;
                Assert.AreEqual(1, Math.Abs(gridNode2.HeadLocation.X - gridNodeChild.HeadLocation.X) + Math.Abs(gridNode2.HeadLocation.Y - gridNodeChild.HeadLocation.Y));
            }
        }

        [TestMethod]
        public void Children_NoChildrenOnVisitedLocations_ManuallyCheck()
        {
            GridSearchNode parent = _basicWorld3X3.GetInitialSearchNode<GridSearchNode>();
            //GoingToTheGoalPosition
            var gridNode = new GridSearchNode(parent, MoveDirection.Down);
            foreach (var nodeChild in gridNode.Children)
            {
                var gridNodeChild = nodeChild as GridSearchNode;
                Assert.AreNotEqual(parent.HeadLocation, gridNodeChild.HeadLocation);
            }
            var gridNode2 = new GridSearchNode(gridNode, MoveDirection.Right);
            foreach (var nodeChild in gridNode2.Children)
            {
                var gridNodeChild = nodeChild as GridSearchNode;
                Assert.AreNotEqual(parent.HeadLocation, gridNodeChild.HeadLocation);
                Assert.AreNotEqual(gridNode.HeadLocation, gridNodeChild.HeadLocation);
            }

            var gridNode3 = new GridSearchNode(gridNode2, MoveDirection.Up);
            foreach (var nodeChild in gridNode3.Children)
            {
                var gridNodeChild = nodeChild as GridSearchNode;
                Assert.AreNotEqual(parent.HeadLocation, gridNodeChild.HeadLocation);
                Assert.AreNotEqual(gridNode.HeadLocation, gridNodeChild.HeadLocation);
                Assert.AreNotEqual(gridNode2.HeadLocation, gridNodeChild.HeadLocation);
            }
        }

        [TestMethod]
        public void Children_NoChildrenOnBlockedLocations_ManuallyCheck()
        {
            GridSearchNode parent = _basicWorld.GetInitialSearchNode<GridSearchNode>();
            //GoingToTheGoalPosition
            var gridNode = new GridSearchNode(parent, MoveDirection.Right);
            gridNode = new GridSearchNode(gridNode, MoveDirection.Right);
            gridNode = new GridSearchNode(gridNode, MoveDirection.Right);
            Assert.AreEqual(1, gridNode.Children.Count);
            var child = gridNode.Children.First.Value as GridSearchNode;
            Assert.AreNotEqual(new Location(0,4), child.HeadLocation);
            Assert.AreNotEqual(new Location(0,2), child.HeadLocation);
            Assert.AreEqual(new Location(3, 1), child.HeadLocation);
        }

        [TestMethod]
        public void Children_GetChildrenList_RelavantChildrenReturned()
        {
            GridSearchNode parent = _basicWorld.GetInitialSearchNode<GridSearchNode>();
            var childs = parent.Children;
            var sampledChild = parent.Children.Last.Value;
            Assert.IsNotNull(childs);
            Assert.AreEqual(2,childs.Count);
            Assert.AreEqual(1, sampledChild.g);
            Assert.AreEqual(23, sampledChild.h);
            childs = sampledChild.Children;
            sampledChild = sampledChild.Children.Last.Value;
            Assert.IsNotNull(childs);
            Assert.AreEqual(2, childs.Count);
            Assert.AreEqual(2, sampledChild.g);
            Assert.AreEqual(22, sampledChild.h);
            childs = sampledChild.Children;
            sampledChild = sampledChild.Children.Last.Value;
            Assert.IsNotNull(childs);
            Assert.AreEqual(2, childs.Count);
            Assert.AreEqual(3, sampledChild.g);
            Assert.AreEqual(21, sampledChild.h);
        }
    }
}
