
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Grid.Domain;

namespace GridTest
{
    [TestClass]
    public class GridSearchNodeTest
    {
        private static World _basicWorld;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _basicWorld = new World(File.ReadAllText(@"..\..\Grid-10-6-5-4-0.grd"), new UntouchedAroundTheGoalHeuristic());
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
            GridSearchNode parent = _basicWorld.GetInitialSearchNode();
            GridSearchNode gn = new GridSearchNode(parent, MoveDirection.Down);
            Assert.IsNotNull(gn);
        }

        [TestMethod]
        public void fValue_getFvalueForNewNode_EqualsZero()
        {
            GridSearchNode parent = _basicWorld.GetInitialSearchNode();
            Assert.AreEqual(0, parent.g);
        }

        [TestMethod]
        public void fValue_getFvalueForChildNode_Equals1()
        {
            GridSearchNode parent = _basicWorld.GetInitialSearchNode();
            GridSearchNode gn = new GridSearchNode(parent, MoveDirection.Down);
            Assert.AreEqual(1, gn.g);
        }

        [TestMethod]
        public void Children_GetChildrenList_RelavanChildrenReturned()
        {
            GridSearchNode parent = _basicWorld.GetInitialSearchNode();
            var childs = parent.Children;
            var sampledChild = parent.Children.Last.Value;
            Assert.IsNotNull(childs);
            Assert.AreEqual(2,childs.Count);
            Assert.AreEqual(1, sampledChild.g);
            Assert.AreEqual(23, sampledChild.h);
            childs = sampledChild.Children;
            sampledChild = sampledChild.Children.Last.Value;
            Assert.IsNotNull(childs);
            Assert.AreEqual(3, childs.Count);
            Assert.AreEqual(2, sampledChild.g);
            Assert.AreEqual(22, sampledChild.h);
            childs = sampledChild.Children;
            sampledChild = sampledChild.Children.Last.Value;
            Assert.IsNotNull(childs);
            Assert.AreEqual(3, childs.Count);
            Assert.AreEqual(3, sampledChild.g);
            Assert.AreEqual(21, sampledChild.h);
        }
    }
}
