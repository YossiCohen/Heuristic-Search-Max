using System;
using MaSib;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace MaSibTest
{
    [TestClass]
    public class WorldTest
    {
        //Public void MethodUnderTest_Scenario_ExpectedResult
        [TestMethod]
        public void Create_NewWorld()
        {
            World w = new World(3,2);
            Assert.IsNotNull(w);
        }

        [TestMethod]
        public void GetDimentions_ReturnDim()
        {
            World b = new World(3, 2);
            Assert.AreEqual(3,b.Dimentions);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_TooManyDims_ThrowException()
        {
            World b = new World(World.MAX_DIM+1, 2);
        }

        [TestMethod]
        public void ValidPosition_ApprovePositionZero_True()
        {
            World w = new World(3, 2);
            bool valid = w.ValidPosition(0);
            Assert.IsTrue(valid);
        }

        [TestMethod]
        public void ValidPosition_ApprovePositionMaxMinusOne_True()
        {
            World w = new World(World.MAX_DIM, 2);
            int max_location = (int) Math.Pow(2, World.MAX_DIM) - 1;
            bool valid = w.ValidPosition(max_location);
            Assert.IsTrue(valid);
        }

        [TestMethod]
        public void ValidPosition_ApprovePositionEight_False()
        {
            World w = new World(3, 2);
            bool valid = w.ValidPosition(8);
            Assert.IsFalse(valid);
        }
        [TestMethod]
        public void ValidPosition_ApprovePositionMax_False()
        {
            World w = new World(World.MAX_DIM, 2);
            int max_location = (int)Math.Pow(2, World.MAX_DIM);
            bool valid = w.ValidPosition(max_location);
            Assert.IsFalse(valid);
        }

        [TestMethod]
        public void IntToBitString_ConvertByDim()
        {
            World w = new World(3, 2);
            var a = w.IntToBitString(5);
            Assert.AreEqual("101", a);
            var b = w.IntToBitString(7);
            Assert.AreEqual("111", b);
            var c = w.IntToBitString(2);
            Assert.AreEqual("010", c);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void IntToBitString_ConvertFailWithException()
        {
            World w = new World(3, 2);
            var a = w.IntToBitString(50);
        }

        [TestMethod]
        public void IntArrToBitString_ConvertByDim()
        {
            World w = new World(3, 2);
            var arr = new int[]{1, 3, 0, 7};
            var str = w.IntArrToBitString(arr);
            Assert.AreEqual("001-011-000-111", str);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void IntArrToBitString_ConvertFailWithException()
        {
            World w = new World(3, 2);
            var arr = new int[] { 111, 3, 0, 7 };
            var str = w.IntArrToBitString(arr);
        }

        [TestMethod]
        public void HammingDistance_GetDistanceThreeTwo_DistanceIsOneTrue()
        {
            World w = new World(3, 2);
            int a = 3; // 011
            int b = 2; // 010
            Assert.AreEqual(1, w.HammingDistance(a,b));
        }

        [TestMethod]
        public void HammingDistance_GetDistanceSevenTwo_DistanceIsTwoTrue()
        {
            World w = new World(3, 2);
            int a = 7; // 111
            int b = 2; // 010
            Assert.AreEqual(2, w.HammingDistance(a, b));
        }

        [TestMethod]
        public void HammingDistance_GetDistanceSevenZero_DistanceIsThreeTrue()
        {
            World w = new World(3, 2);
            int a = 7; // 111
            int b = 0; // 010
            Assert.AreEqual(3, w.HammingDistance(a, b));
        }
    }
}
