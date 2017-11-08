using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Grid.Domain;

namespace GridTest
{
    [TestClass]
    public class WorldTest
    {
        private static string _gridAstr;
        private static string _gridBstr;
        private static string _gridCstr;
        private static string _badGridNoStart;
        private static string _badGridNoGoal;
        private static string _badGridSizes;
        private static string _badGridSmall;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _gridAstr = File.ReadAllText(@"..\..\Grid-10-6-5-4-0.grd");
            _gridBstr = File.ReadAllText(@"..\..\Clean_Grid_5x5BasicBlocked.grd");
            _gridCstr = File.ReadAllText(@"..\..\Grid-10-6-5-4-1.grd");
            _badGridNoGoal = File.ReadAllText(@"..\..\BadGridSizesNoGoal.grd");
            _badGridNoStart = File.ReadAllText(@"..\..\BadGridSizesNoStart.grd");
            _badGridSizes = File.ReadAllText(@"..\..\BadGridSizes.grd");
            _badGridSmall = File.ReadAllText(@"..\..\BadGridSmall.grd");
        }

        [TestMethod]
        public void Constructor_ReturnsValue_NotNull()
        {
            World w = new World(_gridAstr, new NoneHeuristic());
            Assert.IsNotNull(w);
        }

        [TestMethod]
        public void Constructor_GetRightDimentions_SizeOK()
        {
            World w = new World(_gridAstr, new NoneHeuristic());
            Assert.AreEqual(w.Height,5);
            Assert.AreEqual(w.Width, 6);
        }

        [TestMethod]
        [ExpectedException(typeof(GridTooSmallException), "Grid too small")]
        public void Constructor_TinyIrrelevantGrid_TooSmallException()
        {
            World unused = new World(_badGridSmall, new NoneHeuristic());
        }

        [TestMethod]
        [ExpectedException(typeof(GridWithDifferentLinesSize))]
        public void Constructor_BadInput_BadGridSizesException()
        {
            World unused = new World(_badGridSizes, new NoneHeuristic());
        }

        [TestMethod]
        [ExpectedException(typeof(GridGoalNotFoundException))]
        public void Constructor_BadInput_BadGridNoGoalException()
        {
            World unused = new World(_badGridNoGoal, new NoneHeuristic());
        }

        [TestMethod]
        [ExpectedException(typeof(GridStartNotFoundException))]
        public void Constructor_BadInput_BadGridNoStartException()
        {
            World unused = new World(_badGridNoStart, new NoneHeuristic());
        }

        [TestMethod]
        public void IsBlocked_RecognizeBlockedAndNonBlockeLocations_AllOK()
        {
            World w = new World(_gridAstr, new NoneHeuristic());
            Assert.AreEqual(w.Height, 5);
            Assert.AreEqual(w.Width, 6);
            Assert.IsFalse(w.IsBlocked(new Location(3, 0)));
            Assert.IsTrue(w.IsBlocked(new Location(4, 0)));
            Assert.IsFalse(w.IsBlocked(new Location(5, 0)));
        }

        [TestMethod]
        public void GetInitialLocation_Sanity_ReturnsInitialState()
        {
            World w = new World(_gridAstr, new NoneHeuristic());
            GridSearchNode s = w.GetInitialSearchNode();
            Assert.IsNotNull(s);
        }

        [TestMethod]
        public void LinearSize_Sanity_ReturnsRightValue30()
        {
            World w = new World(_gridAstr, new NoneHeuristic());
            Assert.AreEqual(30, w.LinearSize);
        }

        [TestMethod]
        public void LinearSize_Sanity_ReturnsRightValue25()
        {
            World w = new World(_gridBstr, new NoneHeuristic());
            Assert.AreEqual(25, w.LinearSize);
        }

        [TestMethod]
        public void GoalAndStart_PositionReadCorrectly_PositionGood()
        {
            World w = new World(_gridCstr, new NoneHeuristic());
            Assert.AreEqual(new Location(1,2), w.Start);
            Assert.AreEqual(new Location(2,3), w.Goal);
        }

    }
}
