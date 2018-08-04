using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Grid.Domain;

namespace GridTest
{
    [TestClass]
    public class BiconnectedComponentsHeuristicTest
    {
        private static World _basicWorldV1;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _basicWorldV1 = new World(File.ReadAllText(@"..\..\Grid_5x5BiconnectedComponentsHeuristicV1.grd"), new BiconnectedComponentsHeuristic());
        }

        [TestMethod]
        public void gValue_getFvalueForChildNode_Equals24()
        {
            GridSearchNode initialState = _basicWorldV1.GetInitialSearchNode<GridSearchNode>();
            Assert.AreEqual(24, initialState.h);
        }

    }
}
