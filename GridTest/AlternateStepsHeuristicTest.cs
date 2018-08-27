using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Grid.Domain;

namespace GridTest
{
    [TestClass]
    public class AlternateStepsHeuristicTest
    {
        private static World _basicWorld2;
        private static World _basicWorld3;
        private static World _basicWorld4a;
        private static World _basicWorld4b;
        private static World _basicWorld5a;
        private static World _basicWorld5b;
        private static World _basicWorld5c;
        private static World _basicWorld5d;
        private static World _basicWorld5e;
        private static World _basicWorld5f;
        private static World _basicWorld5g;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _basicWorld2 = new World(File.ReadAllText(@"..\..\AlternateStepsHeuristic_Test_2x2.grd"), new AlternateStepsHeuristic());
            _basicWorld3 = new World(File.ReadAllText(@"..\..\AlternateStepsHeuristic_Test_3x3.grd"), new AlternateStepsHeuristic());
            _basicWorld4a = new World(File.ReadAllText(@"..\..\AlternateStepsHeuristic_Test_4x4A.grd"), new AlternateStepsHeuristic());
            _basicWorld4b = new World(File.ReadAllText(@"..\..\AlternateStepsHeuristic_Test_4x4B.grd"), new AlternateStepsHeuristic());
            _basicWorld5a = new World(File.ReadAllText(@"..\..\AlternateStepsHeuristic_Test_5x5_B2_E0_O2.grd"), new AlternateStepsHeuristic());
            _basicWorld5b = new World(File.ReadAllText(@"..\..\AlternateStepsHeuristic_Test_5x5_B2_E2_O0_V1.grd"), new AlternateStepsHeuristic());
            _basicWorld5c = new World(File.ReadAllText(@"..\..\AlternateStepsHeuristic_Test_5x5_B2_E2_O0_V2.grd"), new AlternateStepsHeuristic());
            _basicWorld5d = new World(File.ReadAllText(@"..\..\AlternateStepsHeuristic_Test_5x5_B3_E1_O2.grd"), new AlternateStepsHeuristic());
            _basicWorld5e = new World(File.ReadAllText(@"..\..\AlternateStepsHeuristic_Test_5x5_B3_E3_O0.grd"), new AlternateStepsHeuristic());
            _basicWorld5f = new World(File.ReadAllText(@"..\..\AlternateStepsHeuristic_Test_5x5_B4_E0_O4.grd"), new AlternateStepsHeuristic());
            _basicWorld5g = new World(File.ReadAllText(@"..\..\AlternateStepsHeuristic_Test_5x5_B4_E4_O0.grd"), new AlternateStepsHeuristic());
        }

        [TestMethod]
        public void IsEvenLocation_ReturnsCorrectResult_OnLocationsSet()
        {
            var h = new AlternateStepsHeuristic();
            Location loc = new Location(0, 0);
            Assert.IsTrue(h.IsEvenLocation(loc));
            loc = new Location(5, 7);
            Assert.IsTrue(h.IsEvenLocation(loc));
            loc = new Location(0, 1);
            Assert.IsFalse(h.IsEvenLocation(loc));
            loc = new Location(1, 0);
            Assert.IsFalse(h.IsEvenLocation(loc));
            loc = new Location(1, 8);
            Assert.IsFalse(h.IsEvenLocation(loc));
        }

        [TestMethod]
        public void hValue_heuristicFor2x2_Equals2()
        {
            GridSearchNode initialState = _basicWorld2.GetInitialSearchNode<GridSearchNode>();
            Assert.AreEqual(2, initialState.h);
        }
        [TestMethod]
        public void hValue_heuristicFor3x3_Equals8()
        {
            GridSearchNode initialState = _basicWorld3.GetInitialSearchNode<GridSearchNode>();
            Assert.AreEqual(8, initialState.h);
        }

        [TestMethod]
        public void hValue_heuristicFor4x4A_Equals14()
        {
            GridSearchNode initialState = _basicWorld4a.GetInitialSearchNode<GridSearchNode>();
            Assert.AreEqual(14, initialState.h);
        }

        [TestMethod]
        public void hValue_heuristicFor4x4B_Equals15()
        {
            GridSearchNode initialState = _basicWorld4b.GetInitialSearchNode<GridSearchNode>();
            Assert.AreEqual(15, initialState.h);
        }

        [TestMethod]
        public void hValue_heuristicFor5x5A_Equals20()
        {
            GridSearchNode initialState = _basicWorld5a.GetInitialSearchNode<GridSearchNode>();
            Assert.AreEqual(20, initialState.h);
        }

        [TestMethod]
        public void hValue_heuristicFor5x5B_Equals20()
        {
            GridSearchNode initialState = _basicWorld5b.GetInitialSearchNode<GridSearchNode>();
            Assert.AreEqual(20, initialState.h);
        }

        [TestMethod]
        public void hValue_heuristicFor5x5C_Equals21()
        {
            GridSearchNode initialState = _basicWorld5c.GetInitialSearchNode<GridSearchNode>();
            Assert.AreEqual(21, initialState.h);
        }

        [TestMethod]
        public void hValue_heuristicFor5x5D_Equals20()
        {
            GridSearchNode initialState = _basicWorld5d.GetInitialSearchNode<GridSearchNode>();
            Assert.AreEqual(20, initialState.h);
        }

        [TestMethod]
        public void hValue_heuristicFor5x5E_Equals18()
        {
            GridSearchNode initialState = _basicWorld5e.GetInitialSearchNode<GridSearchNode>();
            Assert.AreEqual(18, initialState.h);
        }

        [TestMethod]
        public void hValue_heuristicFor5x5F_Equals20()
        {
            GridSearchNode initialState = _basicWorld5f.GetInitialSearchNode<GridSearchNode>();
            Assert.AreEqual(16, initialState.h);
        }

        [TestMethod]
        public void hValue_heuristicFor5x5G_Equals20()
        {
            GridSearchNode initialState = _basicWorld5g.GetInitialSearchNode<GridSearchNode>();
            Assert.AreEqual(16, initialState.h);
        }
        
    }
}
