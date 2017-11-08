
using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Grid.Domain;
using MaxSearchAlg;

namespace GridTest
{
    [TestClass]
    public class GridAStarMaxTest
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
        public void Create_NewAStarGrid()
        {
            GridSearchNode initialState = _basicWorld3.GetInitialSearchNode();
            AStarMax astar = new AStarMax(initialState);
            Assert.IsNotNull(astar);
            astar.Run(1);
            var maxGoal = astar.GetMaxGoal();
            Assert.IsNotNull(maxGoal);
        }
    }
}
