
using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Grid.Domain;

namespace GridTest
{
    [TestClass]
    public class RsdGridSearchNodeTest
    {
        private static World _basicWorld;
        private static World _basicWorld3X3;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _basicWorld3X3 = new World(File.ReadAllText(@"..\..\33.grd"), new UntouchedAroundTheGoalHeuristic());
            _basicWorld = new World(File.ReadAllText(@"..\..\Grid-10-6-5-4-0.grd"), new UntouchedAroundTheGoalHeuristic());
        }

        [TestMethod]
        public void Constructor_CreatesState_Created()
        {
            GridSearchNode searchNode = new RsdGridSearchNode(_basicWorld);
            Assert.IsNotNull(searchNode);
        }

        [TestMethod]
        public void Constructor_CreatesStateWithParent_Created()
        {
            RsdGridSearchNode initialSearchNode = _basicWorld.GetInitialSearchNode<RsdGridSearchNode>();
            RsdGridSearchNode gn = new RsdGridSearchNode(initialSearchNode, MoveDirection.Down);
            Assert.IsNotNull(gn);
        }

        [TestMethod]
        public void fValue_getFvalueForNewNode_EqualsZero()
        {
            RsdGridSearchNode initialSearchNode = _basicWorld.GetInitialSearchNode<RsdGridSearchNode>();
            Assert.AreEqual(0, initialSearchNode.g);
        }

        [TestMethod]
        public void fValue_getFvalueForChildNode_Equals1()
        {
            RsdGridSearchNode initialSearchNode = _basicWorld.GetInitialSearchNode<RsdGridSearchNode>();
            RsdGridSearchNode gn = new RsdGridSearchNode(initialSearchNode, MoveDirection.Down);
            Assert.AreEqual(1, gn.g);
        }

        [TestMethod]
        public void Children_GoalHaveNoChildren_CountEquals0()
        {
            RsdGridSearchNode initialSearchNode = _basicWorld3X3.GetInitialSearchNode<RsdGridSearchNode>();
            //GoingToTheGoalPosition
            var gridNode = new RsdGridSearchNode(initialSearchNode, MoveDirection.Down);
            gridNode = new RsdGridSearchNode(gridNode, MoveDirection.Down);
            gridNode = new RsdGridSearchNode(gridNode, MoveDirection.Right);
            gridNode = new RsdGridSearchNode(gridNode, MoveDirection.Right);
            Assert.AreEqual(0, gridNode.Children.Count);
        }

        [TestMethod]
        public void Children_AreAllwaysNearThierParents_DeltaDistanceAlways1()
        {
            RsdGridSearchNode initialSearchNode = _basicWorld3X3.GetInitialSearchNode<RsdGridSearchNode>();
            //GoingToTheGoalPosition
            var gridNode = new RsdGridSearchNode(initialSearchNode, MoveDirection.Down);
            foreach (var NodeChild in gridNode.Children)
            {
                var gridNodeChild = NodeChild as RsdGridSearchNode;
                Assert.AreEqual(1, Math.Abs(gridNode.HeadLocation.X - gridNodeChild.HeadLocation.X) + Math.Abs(gridNode.HeadLocation.Y - gridNodeChild.HeadLocation.Y));
            }
            var gridNode2 = new RsdGridSearchNode(gridNode, MoveDirection.Right);
            foreach (var nodeChild in gridNode2.Children)
            {
                var gridNodeChild = nodeChild as RsdGridSearchNode;
                Assert.AreEqual(1, Math.Abs(gridNode2.HeadLocation.X - gridNodeChild.HeadLocation.X) + Math.Abs(gridNode2.HeadLocation.Y - gridNodeChild.HeadLocation.Y));
            }
        }

        [TestMethod]
        public void Children_NoChildrenOnVisitedLocations_ManuallyCheck()
        {
            RsdGridSearchNode initialSearchNode = _basicWorld3X3.GetInitialSearchNode<RsdGridSearchNode>();
            //GoingToTheGoalPosition
            var gridNode = new RsdGridSearchNode(initialSearchNode, MoveDirection.Down);
            foreach (var nodeChild in gridNode.Children)
            {
                var gridNodeChild = nodeChild as RsdGridSearchNode;
                Assert.AreNotEqual(initialSearchNode.HeadLocation, gridNodeChild.HeadLocation);
            }
            var gridNode2 = new RsdGridSearchNode(gridNode, MoveDirection.Right);
            foreach (var nodeChild in gridNode2.Children)
            {
                var gridNodeChild = nodeChild as RsdGridSearchNode;
                Assert.AreNotEqual(initialSearchNode.HeadLocation, gridNodeChild.HeadLocation);
                Assert.AreNotEqual(gridNode.HeadLocation, gridNodeChild.HeadLocation);
            }

            var gridNode3 = new RsdGridSearchNode(gridNode2, MoveDirection.Up);
            foreach (var nodeChild in gridNode3.Children)
            {
                var gridNodeChild = nodeChild as RsdGridSearchNode;
                Assert.AreNotEqual(initialSearchNode.HeadLocation, gridNodeChild.HeadLocation);
                Assert.AreNotEqual(gridNode.HeadLocation, gridNodeChild.HeadLocation);
                Assert.AreNotEqual(gridNode2.HeadLocation, gridNodeChild.HeadLocation);
            }
        }

        [TestMethod]
        public void Children_NoChildrenOnBlockedLocations_ManuallyCheck()
        {
            RsdGridSearchNode initialSearchNode = _basicWorld3X3.GetInitialSearchNode<RsdGridSearchNode>();
            //GoingToTheGoalPosition
            var gridNode = new RsdGridSearchNode(initialSearchNode, MoveDirection.Right);
            gridNode = new RsdGridSearchNode(gridNode, MoveDirection.Right);
            gridNode = new RsdGridSearchNode(gridNode, MoveDirection.Right);
            Assert.AreEqual(1, gridNode.Children.Count);
            var child = gridNode.Children.First.Value as RsdGridSearchNode;
            Assert.AreNotEqual(new Location(0,4), child.HeadLocation);
            Assert.AreNotEqual(new Location(0,2), child.HeadLocation);
            Assert.AreEqual(new Location(3, 1), child.HeadLocation);
        }

        [TestMethod]
        public void Children_GetChildrenList_RelavantChildrenReturned()
        {
            RsdGridSearchNode initialSearchNode = _basicWorld.GetInitialSearchNode<RsdGridSearchNode>();
            var childs = initialSearchNode.Children;
            var sampledChild = initialSearchNode.Children.Last.Value;
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
