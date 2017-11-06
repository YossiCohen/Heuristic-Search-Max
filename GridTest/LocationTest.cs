
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Grid.Domain;

namespace GridTest
{
    [TestClass]
    public class LocationTest
    {
        [TestMethod]
        public void Create_EmptyLocation_Zero()
        {
            Location loc = new Location();
            Assert.IsNotNull(loc);
            Assert.AreEqual(0, loc.X);
            Assert.AreEqual(0, loc.Y);
        }

        [TestMethod]
        public void Create_Location_ByValue()
        {
            Location loc = new Location(1,2);
            Assert.IsNotNull(loc);
            Assert.AreEqual(1, loc.X);
            Assert.AreEqual(2, loc.Y);
        }

        [TestMethod]
        public void Clone_Location_Equals()
        {
            Location loc1 = new Location(1, 2);
            Location loc2 = new Location(loc1);
            Assert.IsNotNull(loc2);
            Assert.AreEqual(loc1.X, loc2.X);
            Assert.AreEqual(loc1.Y, loc2.Y);
            Assert.IsTrue(loc1.Equals(loc2));
        }

        [TestMethod]
        public void ToString_Location()
        {
            Location loc1 = new Location(1, 2);
            string s = loc1.ToString();
            Assert.AreEqual("(1,2)", s);
        }
    }
}
