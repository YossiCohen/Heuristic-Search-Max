
using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Grid.Domain;

namespace GridTest
{
    [TestClass]
    public class UntouchedAroundTheGoalHeuristicTest
    {
        private static World _basicWorld;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _basicWorld = new World(File.ReadAllText(@"..\..\Grid-10-6-5-4-0.grd"), new UntouchedAroundTheGoalHeuristic());
        }

        [TestMethod]
        public void gValue_getFvalueForChildNode_Equals1()
        {
            GridSearchNode initialState = _basicWorld.GetInitialSearchNode();
            Assert.AreEqual(24, initialState.g);
        }
    }
}
