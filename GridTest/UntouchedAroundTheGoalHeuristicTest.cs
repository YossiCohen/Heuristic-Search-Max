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

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _basicWorld = new World(File.ReadAllText(@"..\..\Grid-10-6-5-4-0.grd"), new UntouchedAroundTheGoalHeuristic());
            _basicWorld2 = new World(File.ReadAllText(@"..\..\Grid-10-6-5-4-1.grd"), new UntouchedAroundTheGoalHeuristic());
            _basicWorld3 = new World(File.ReadAllText(@"..\..\Clean_Grid_5x5BasicBlocked.grd"), new UntouchedAroundTheGoalHeuristic());
        }

        [TestMethod]
        public void gValue_getFvalueForChildNode_Equals24()
        {
            GridSearchNode initialState = _basicWorld.GetInitialSearchNode<GridSearchNode>();
            Assert.AreEqual(24, initialState.h);
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
