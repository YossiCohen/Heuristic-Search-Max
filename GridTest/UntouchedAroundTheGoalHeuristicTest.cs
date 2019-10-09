using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Grid.Domain;

namespace GridTest
{
    [TestClass]
    public class UntouchedAroundTheGoalHeuristicTest
    {
        private static World _basicWorld;
        private static World _basicWorld2;
        private static World _basicWorld3;
        private static World _basicWorld3X3Life;
        private static World _basicWorld3X3;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _basicWorld = new World(File.ReadAllText(@"..\..\Grid-10-6-5-4-0.grd"), new UntouchedAroundTheGoalHeuristic(), WorldType.Uniform);
            _basicWorld2 = new World(File.ReadAllText(@"..\..\Grid-10-6-5-4-1.grd"), new UntouchedAroundTheGoalHeuristic(), WorldType.Uniform);
            _basicWorld3 = new World(File.ReadAllText(@"..\..\Clean_Grid_5x5BasicBlocked.grd"), new UntouchedAroundTheGoalHeuristic(), WorldType.Uniform);
            _basicWorld3X3Life = new World(File.ReadAllText(@"..\..\33.grd"), new UntouchedAroundTheGoalHeuristic(), WorldType.Life);
            _basicWorld3X3 = new World(File.ReadAllText(@"..\..\33.grd"), new UntouchedAroundTheGoalHeuristic(), WorldType.Uniform);
        }

        [TestMethod]
        public void gValue_getFvalueForChildNode_Equals24()
        {
            GridSearchNode initialState = _basicWorld.GetInitialSearchNode<GridSearchNode>();
            Assert.AreEqual(24, initialState.h);
        }

        [TestMethod]
        public void gValueLife_getFvalueForChildNode_Equals17()
        {
            GridSearchNode initialState = _basicWorld3X3Life.GetInitialSearchNode<GridSearchNode>();
            Assert.AreEqual(9, initialState.h);
        }

        [TestMethod]
        public void gValue_getFvalueForChildNode_Equals8()
        {
            GridSearchNode initialState = _basicWorld3X3.GetInitialSearchNode<GridSearchNode>();
            Assert.AreEqual(8, initialState.h);
        }

        [TestMethod]
        public void gValue_getFvalueForChildNode_Equals24b()
        {
            GridSearchNode initialState = _basicWorld2.GetInitialSearchNode<GridSearchNode>();
            Assert.AreEqual(24, initialState.h);
        }

        [TestMethod]
        public void gValue_getFvalueForChildNode_Equals20()
        {
            GridSearchNode initialState = _basicWorld3.GetInitialSearchNode<GridSearchNode>();
            Assert.AreEqual(20, initialState.h);
        }
    }
}
