
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
        public void GetMovedLocation_Left_MoveLeft()
        {
            Location locSource = new Location(1, 2);
            Location locMoved = locSource.GetMovedLocation(MoveDirection.Left);
            Location locDestination = new Location(0,2);
            Assert.AreEqual(locDestination, locMoved);
        }

        [TestMethod]
        public void GetMovedLocation_Right_MoveRight()
        {
            Location locSource = new Location(1, 2);
            Location locMoved = locSource.GetMovedLocation(MoveDirection.Right);
            Location locDestination = new Location(2,2);
            Assert.AreEqual(locDestination, locMoved);
        }

        [TestMethod]
        public void GetMovedLocation_Up_MoveUp()
        {
            Location locSource = new Location(1, 2);
            Location locMoved = locSource.GetMovedLocation(MoveDirection.Up);
            Location locDestination = new Location(1,1);
            Assert.AreEqual(locDestination, locMoved);
        }

        [TestMethod]
        public void GetMovedLocation_Down_MoveDown()
        {
            Location locSource = new Location(1, 2);
            Location locMoved = locSource.GetMovedLocation(MoveDirection.Down);
            Location locDestination = new Location(1,3);
            Assert.AreEqual(locDestination, locMoved);
        }

        [TestMethod]
        public void GetMovedLocation_Stay_NotMoving()
        {
            Location locSource = new Location(1, 2);
            Location locMoved = locSource.GetMovedLocation(MoveDirection.Wait);
            Location locDestination = new Location(1,2);
            Assert.AreEqual(locDestination, locMoved);
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
