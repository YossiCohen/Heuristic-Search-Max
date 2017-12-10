using System.IO;
using Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Grid.Domain;

namespace GridTest
{
    [TestClass]
    public class RsdUntouchedAroundTheGoalHeuristicTest
    {
        private static World _basicWorld;
        private static World _basicWorld2;
        private static World _basicWorld3;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _basicWorld = new World(File.ReadAllText(@"..\..\Grid-10-6-5-4-0.grd"), new RsdUntouchedAroundTheGoalHeuristic());
            _basicWorld2 = new World(File.ReadAllText(@"..\..\Grid-10-6-5-4-1.grd"), new RsdUntouchedAroundTheGoalHeuristic());
            _basicWorld3 = new World(File.ReadAllText(@"..\..\Clean_Grid_5x5BasicBlocked.grd"), new RsdUntouchedAroundTheGoalHeuristic());
        }

        [TestMethod]
        public void gValue_getFvalueForChildNode_Equals24()
        {
            RsdGridSearchNode initialState = _basicWorld.GetInitialSearchNode<RsdGridSearchNode>();
            Assert.AreEqual(24, initialState.h);
            Assert.AreEqual(24, HelpMethods.GetCardinality(initialState.Reachable));
            Assert.IsFalse(initialState.Reachable[0]); //(0,0)
            Assert.IsTrue(initialState.Reachable[1]); //(1,0)
            Assert.IsTrue(initialState.Reachable[2]); //(2,0)
            Assert.IsTrue(initialState.Reachable[3]); //(3,0)
            Assert.IsFalse(initialState.Reachable[4]); //(4,0)
            Assert.IsFalse(initialState.Reachable[5]); //(5,0)
            Assert.IsTrue(initialState.Reachable[6]); //(0,1)
            Assert.IsTrue(initialState.Reachable[7]); //(1,1)
            Assert.IsTrue(initialState.Reachable[8]); //(2,1)
            Assert.IsTrue(initialState.Reachable[9]); //(3,1)
            Assert.IsFalse(initialState.Reachable[10]); //(4,1)
            Assert.IsFalse(initialState.Reachable[11]); //(5,1)
            Assert.IsTrue(initialState.Reachable[12]); //(0,2)
            Assert.IsTrue(initialState.Reachable[13]); //(1,2)
            Assert.IsTrue(initialState.Reachable[14]); //(2,2)
            Assert.IsTrue(initialState.Reachable[15]); //(3,2)
            Assert.IsTrue(initialState.Reachable[16]); //(4,2)
            Assert.IsFalse(initialState.Reachable[17]); //(5,2)
            Assert.IsTrue(initialState.Reachable[18]); //(0,3)
            Assert.IsTrue(initialState.Reachable[19]); //(1,3)
            Assert.IsTrue(initialState.Reachable[20]); //(2,3)
            Assert.IsTrue(initialState.Reachable[21]); //(3,3)
            Assert.IsTrue(initialState.Reachable[22]); //(4,3)
            Assert.IsTrue(initialState.Reachable[23]); //(5,3)
            Assert.IsTrue(initialState.Reachable[24]); //(0,4)
            Assert.IsTrue(initialState.Reachable[25]); //(1,4)
            Assert.IsTrue(initialState.Reachable[26]); //(2,4)
            Assert.IsTrue(initialState.Reachable[27]); //(3,4)
            Assert.IsTrue(initialState.Reachable[28]); //(4,4)
            Assert.IsTrue(initialState.Reachable[29]); //(5,4)
        }

        [TestMethod]
        public void gValue_getFvalueForChildNode_Equals24b()
        {
            RsdGridSearchNode initialState = _basicWorld2.GetInitialSearchNode<RsdGridSearchNode>();
            Assert.AreEqual(24, initialState.h);
            Assert.AreEqual(24, HelpMethods.GetCardinality(initialState.Reachable));
            Assert.IsTrue(initialState.Reachable[0]); //(0,0)
            Assert.IsTrue(initialState.Reachable[1]); //(1,0)
            Assert.IsTrue(initialState.Reachable[2]); //(2,0)
            Assert.IsTrue(initialState.Reachable[3]); //(3,0)
            Assert.IsFalse(initialState.Reachable[4]); //(4,0)
            Assert.IsFalse(initialState.Reachable[5]); //(5,0)
            Assert.IsTrue(initialState.Reachable[6]); //(0,1)
            Assert.IsTrue(initialState.Reachable[7]); //(1,1)
            Assert.IsTrue(initialState.Reachable[8]); //(2,1)
            Assert.IsTrue(initialState.Reachable[9]); //(3,1)
            Assert.IsFalse(initialState.Reachable[10]); //(4,1)
            Assert.IsFalse(initialState.Reachable[11]); //(5,1)
            Assert.IsTrue(initialState.Reachable[12]); //(0,2)
            Assert.IsFalse(initialState.Reachable[13]); //(1,2)
            Assert.IsTrue(initialState.Reachable[14]); //(2,2)
            Assert.IsTrue(initialState.Reachable[15]); //(3,2)
            Assert.IsTrue(initialState.Reachable[16]); //(4,2)
            Assert.IsFalse(initialState.Reachable[17]); //(5,2)
            Assert.IsTrue(initialState.Reachable[18]); //(0,3)
            Assert.IsTrue(initialState.Reachable[19]); //(1,3)
            Assert.IsTrue(initialState.Reachable[20]); //(2,3)
            Assert.IsTrue(initialState.Reachable[21]); //(3,3)
            Assert.IsTrue(initialState.Reachable[22]); //(4,3)
            Assert.IsTrue(initialState.Reachable[23]); //(5,3)
            Assert.IsTrue(initialState.Reachable[24]); //(0,4)
            Assert.IsTrue(initialState.Reachable[25]); //(1,4)
            Assert.IsTrue(initialState.Reachable[26]); //(2,4)
            Assert.IsTrue(initialState.Reachable[27]); //(3,4)
            Assert.IsTrue(initialState.Reachable[28]); //(4,4)
            Assert.IsTrue(initialState.Reachable[29]); //(5,4)
        }

        [TestMethod]
        public void gValue_getFvalueForChildNode_Equals20()
        {
            RsdGridSearchNode initialState = _basicWorld3.GetInitialSearchNode<RsdGridSearchNode>();
            Assert.AreEqual(20, initialState.h);
            Assert.AreEqual(20, HelpMethods.GetCardinality(initialState.Reachable));
            Assert.IsFalse(initialState.Reachable[0]); //(0,0)
            Assert.IsTrue(initialState.Reachable[1]); //(1,0)
            Assert.IsTrue(initialState.Reachable[2]); //(2,0)
            Assert.IsTrue(initialState.Reachable[3]); //(3,0)
            Assert.IsTrue(initialState.Reachable[4]); //(4,0)
            Assert.IsTrue(initialState.Reachable[5]); //(5,0)
        }
    }
}
