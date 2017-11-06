using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Grid.Domain;

namespace GridTest
{
    [TestClass]
    public class WorldTest
    {
        private static string gridAstr;
        private static string gridBstr;
        private static string badGridNoStart;
        private static string badGridNoGoal;
        private static string badGridSizes;
        private static string badGridSmall;

        [ClassInitialize]
        public static void TestInitialize(TestContext context)
        {
            gridAstr = File.ReadAllText(@"..\..\Grid-10-6-5-4-0.grd");
            gridBstr = File.ReadAllText(@"..\..\Clean_Grid_5x5BasicBlocked.grd");
            badGridNoGoal = File.ReadAllText(@"..\..\BadGridSizesNoGoal.grd");
            badGridNoStart = File.ReadAllText(@"..\..\BadGridSizesNoStart.grd");
            badGridSizes = File.ReadAllText(@"..\..\BadGridSizes.grd");
            badGridSmall = File.ReadAllText(@"..\..\BadGridSmall.grd");
        }

        [TestMethod]
        public void Create_World_NotNull()
        {
            World w = new World(gridAstr);
            Assert.IsNotNull(w);

        }

        [TestMethod]
        public void Create_World_SizeOK()
        {
            World w = new World(gridAstr);
            Assert.AreEqual(w.Height,5);
            Assert.AreEqual(w.Width, 6);
        }

        [TestMethod]
        [ExpectedException(typeof(GridTooSmallException), "Grid too small")]
        public void Create_World_TooSmall()
        {
            World w = new World(badGridSmall);
        }

        [TestMethod]
        [ExpectedException(typeof(GridWithDifferentLinesSize))]
        public void Create_World_BadGridSizes()
        {
            World w = new World(badGridSizes);
        }

        [TestMethod]
        [ExpectedException(typeof(GridGoalNotFoundException))]
        public void Create_World_BadGridNoGoal()
        {
            World w = new World(badGridNoGoal);
        }

        [TestMethod]
        [ExpectedException(typeof(GridStartNotFoundException))]
        public void Create_World_BadGridNoStart()
        {
            World w = new World(badGridNoStart);
        }


        [TestMethod]
        public void Create_World_BlockedOK()
        {
            World w = new World(gridAstr);
            Assert.AreEqual(w.Height, 5);
            Assert.AreEqual(w.Width, 6);
            Assert.IsFalse(w.isBlocked(new Location(3, 0)));
            Assert.IsTrue(w.isBlocked(new Location(4, 0)));
            Assert.IsFalse(w.isBlocked(new Location(5, 0)));
        }

    }
}
