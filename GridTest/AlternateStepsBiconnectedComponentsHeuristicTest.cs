using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Grid.Domain;
using MaxSearchAlg;
using System;

namespace GridTest
{
    [TestClass]
    public class AlternateStepsBiconnectedComponentsHeuristicTest
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
        private static World _specialCase01;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _basicWorld2 = new World(File.ReadAllText(@"..\..\AlternateStepsHeuristic_Test_2x2.grd"), new AlternateStepsBiconnectedComponentsHeuristic());
            _basicWorld3 = new World(File.ReadAllText(@"..\..\AlternateStepsHeuristic_Test_3x3.grd"), new AlternateStepsBiconnectedComponentsHeuristic());
            _basicWorld4a = new World(File.ReadAllText(@"..\..\AlternateStepsHeuristic_Test_4x4A.grd"), new AlternateStepsBiconnectedComponentsHeuristic());
            _basicWorld4b = new World(File.ReadAllText(@"..\..\AlternateStepsHeuristic_Test_4x4B.grd"), new AlternateStepsBiconnectedComponentsHeuristic());
            _basicWorld5a = new World(File.ReadAllText(@"..\..\AlternateStepsHeuristic_Test_5x5_B2_E0_O2.grd"), new AlternateStepsBiconnectedComponentsHeuristic());
            _basicWorld5b = new World(File.ReadAllText(@"..\..\AlternateStepsHeuristic_Test_5x5_B2_E2_O0_V1.grd"), new AlternateStepsBiconnectedComponentsHeuristic());
            _basicWorld5c = new World(File.ReadAllText(@"..\..\AlternateStepsHeuristic_Test_5x5_B2_E2_O0_V2.grd"), new AlternateStepsBiconnectedComponentsHeuristic());
            _basicWorld5d = new World(File.ReadAllText(@"..\..\AlternateStepsHeuristic_Test_5x5_B3_E1_O2.grd"), new AlternateStepsBiconnectedComponentsHeuristic());
            _basicWorld5e = new World(File.ReadAllText(@"..\..\AlternateStepsHeuristic_Test_5x5_B3_E3_O0.grd"), new AlternateStepsBiconnectedComponentsHeuristic());
            _basicWorld5f = new World(File.ReadAllText(@"..\..\AlternateStepsHeuristic_Test_5x5_B4_E0_O4.grd"), new AlternateStepsBiconnectedComponentsHeuristic());
            _basicWorld5g = new World(File.ReadAllText(@"..\..\AlternateStepsHeuristic_Test_5x5_B4_E4_O0.grd"), new AlternateStepsBiconnectedComponentsHeuristic());
        }

        [TestMethod]
        public void IsEvenLocation_ReturnsCorrectResult_OnLocationsSet()
        {
            var h = new AlternateStepsBiconnectedComponentsHeuristic();
            //when grid is oddXodd we get alternate linear location all the way
            bool shouldBeEven = true;
            for(int i = 0; i < _basicWorld5a.Height; i++)
            {
                for (int j = 0; j < _basicWorld5a.Width; j++)
                {
                    int linearLoc = i* _basicWorld5a.Width + j;
                    Assert.AreEqual(shouldBeEven, h.IsEvenLocation(_basicWorld5a, linearLoc));
                    shouldBeEven = !shouldBeEven;
                }
            }
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
            Assert.AreEqual(14, initialState.h);
        }

        [TestMethod]
        public void Integration_altbccWithRsdBug_ShouldFindSolution()
        {
            _specialCase01 = new World(File.ReadAllText(@"..\..\altbcc-rsd-bug001.grd"), new AlternateStepsBiconnectedComponentsHeuristic());
            AStarMax astar = new AStarMax(_specialCase01.GetInitialSearchNode<RsdGridSearchNode>(), new GoalOnLocation(_specialCase01.Goal));
            Assert.IsNotNull(astar);
            astar.Run(Int32.MaxValue);
            var AstarMaxGoal = astar.GetMaxGoal();
            DfBnbMax dfbnb = new DfBnbMax(_specialCase01.GetInitialSearchNode<GridSearchNode>(), new GoalOnLocation(_specialCase01.Goal));
            Assert.IsNotNull(dfbnb);
            dfbnb.Run(Int32.MaxValue);
            var DfbnbMaxGoal = dfbnb.GetMaxGoal();
            Assert.AreEqual(AstarMaxGoal.g, DfbnbMaxGoal.g);
        }
    }
}
