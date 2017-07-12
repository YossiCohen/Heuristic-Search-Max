using System;
using MaSib;
using MaSib.Domain.SIB;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace MaSibTest
{
    [TestClass]
    public class SnakeTest
    {
        //Public void MethodUnderTest_Scenario_ExpectedResult

        // these are needed on every test
        World world;

        private ISnakeHeuristic heuristicFunc;

        [TestInitialize]
        public void TestInitialize()
        {
            world = new World(4, 2);
            heuristicFunc = new SnakeNoneHeuristic();
        }

        [TestMethod]
        public void Create_NewSnake_NotNull()
        {
            Snake s = new Snake(world, 0, heuristicFunc);
            Assert.IsNotNull(s);
        }

//        [TestMethod]
//        public void Create_NewSnake_ParentIsNull()
//        {
//            Snake s = new Snake(world, 0, heuristicFunc);
//            Assert.IsNull(s.Parent);
//        }

        [TestMethod]
        public void Create_NewSnake_UseDimEqualseZero()
        {
            Snake s = new Snake(world, 0, heuristicFunc, true);
            Assert.AreEqual(0,s.VisitedDim);
        }

        [TestMethod]
        public void Create_NewSnake_NoDimEqualsBoxDims()
        {
            Snake s = new Snake(world, 0, heuristicFunc, false);
            Assert.AreEqual(world.Dimentions, s.VisitedDim);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateNewSnake_BadPosition_Exception()
        {
            Snake s = new Snake(world, 64, heuristicFunc);
            Assert.IsNotNull(s);
        }

        [TestMethod]
        public void Generate_GenerateWithoutDimentions_ReturnsChildSnakes()
        {
            Snake s = new Snake(world, 0, heuristicFunc, true);
            var childrenList = s.Children;
            Assert.IsNotNull(childrenList);
        }

        [TestMethod]
        public void Generate_GenerateWithoutDimentionsAndGValue_ReturnsChildSnakesSomeLevels()
        {
            Snake s = new Snake(world, 0, heuristicFunc, true);
            Assert.AreEqual(1, s.g);
            var lstGen = s.Children;
            Assert.AreEqual(2, lstGen.Last.Value.g);
            Assert.IsNotNull(lstGen);
            var lstGen2 = lstGen.Last.Value.Children;
            Assert.AreEqual(3, lstGen2.Last.Value.g);
            Assert.IsNotNull(lstGen2);
            var lstGen3 = lstGen2.Last.Value.Children;
            Assert.AreEqual(4, lstGen3.Last.Value.g);
            Assert.IsNotNull(lstGen3);
            var lstGen41 = lstGen3.First.Value.Children;
            Assert.AreEqual(5, lstGen41.Last.Value.g);
            Assert.IsNotNull(lstGen41);
        }

        [TestMethod]
        public void SnakeGrow_IncrementDimentionForSmallDim_IncInOne()
        {
            Snake s = new Snake(world, 0, heuristicFunc);
            Snake child = new Snake(s, Snake.FlipBitAt(0, 0), true);
            Assert.AreEqual(1,child.VisitedDim);
        }

        [TestMethod]
        public void SnakeGrow_NotIncrementDimentionForSmallDim_DontIncDim()
        {
            Snake s = new Snake(world, 0, heuristicFunc);
            Snake child = new Snake(s, Snake.FlipBitAt(0, 0), false);
            Assert.AreEqual(0, child.VisitedDim);
        }

        [TestMethod]
        public void SnakeGrow_IncrementDimentionDisabled_SameDimAsParent()
        {
            Snake s = new Snake(world, 0, heuristicFunc, false);
            Snake child = new Snake(s, Snake.FlipBitAt(0, 0), true);
            Assert.AreEqual(s.VisitedDim, child.VisitedDim);
        }

        [TestMethod]
        public void ValidStep_ChecksValidStepNoDim_ReturnsTrueForOne()
        {
            byte a = 0;
            byte b = 1;
            var validStep = Snake.ValidOneStepOfSnake(a, b);
            Assert.IsTrue(validStep);
        }

        [TestMethod]
        public void ValidStep_ChecksValidStepNoDim_ReturnsTrueForTwo()
        {
            byte a = 0;
            byte b = 2;
            var validStep = Snake.ValidOneStepOfSnake(a, b);
            Assert.IsTrue(validStep);
        }

        [TestMethod]
        public void ValidStep_ChecksValidStepNoDim_ReturnsFalseForThree()
        {
            byte a = 0;
            byte b = 3;
            var validStep = Snake.ValidOneStepOfSnake(a, b);
            Assert.IsFalse(validStep);
        }

        [TestMethod]
        public void FlipBitAt_FlipBitAtSpecificPosition_ZeroFlipAtZeroEqualsOne()
        {
            byte a = 0;
            byte b = 0;
            var result = Snake.FlipBitAt(a, b);
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void FlipBitAt_FlipBitAtSpecificPosition_ZeroFlipAtTwoEqualsFour()
        {
            byte a = 0;
            byte b = 2;
            var result = Snake.FlipBitAt(a, b);
            Assert.AreEqual(4, result);
        }

        [TestMethod]
        public void Generate_HeadIsTheLastPartOfTail_HeadsLocatedProperly()
        {
            Snake s = new Snake(world, 0, heuristicFunc, true);
            Assert.AreEqual(s.tail[s.tail.Length -1], s.Head);
            var lstGen = s.Children;
            s = (Snake) lstGen.Last.Value;
            Assert.AreEqual(s.tail[s.tail.Length - 1], s.Head);
            var lstGen2 = lstGen.Last.Value.Children;
            s = (Snake)lstGen2.Last.Value;
            Assert.AreEqual(s.tail[s.tail.Length - 1], s.Head);
            var lstGen3 = lstGen2.Last.Value.Children;
            s = (Snake)lstGen3.Last.Value;
            Assert.AreEqual(s.tail[s.tail.Length - 1], s.Head);
            var lstGen41 = lstGen3.First.Value.Children;
            var lstGen42 = lstGen3.Last.Value.Children;
            s = (Snake)lstGen41.Last.Value;
            Assert.AreEqual(s.tail[s.tail.Length - 1], s.Head);
            s = (Snake)lstGen42.Last.Value;
            Assert.AreEqual(s.tail[s.tail.Length - 1], s.Head);
        }

        [TestMethod]
        public void Generate_AllDecendentsHaveSameParentParts_SameSubSnakes()
        {
            Snake s = new Snake(world, 0, heuristicFunc, true);
            Assert.AreEqual(s.tail[s.tail.Length - 1], s.Head);
            var lstGen = s.Children;
            Snake s1 = (Snake)lstGen.Last.Value;
            for (int i = 0; i < s.tail.Length; i++)
            {
                Assert.AreEqual(s.tail[i], s1.tail[i]);
            }
            var lstGen2 = lstGen.Last.Value.Children;
            Snake s2 = (Snake)lstGen2.Last.Value;
            for (int i = 0; i < s1.tail.Length; i++)
            {
                Assert.AreEqual(s1.tail[i], s2.tail[i]);
            }
            var lstGen3 = lstGen2.Last.Value.Children;
            Snake s3 = (Snake)lstGen3.Last.Value;
            for (int i = 0; i < s2.tail.Length; i++)
            {
                Assert.AreEqual(s2.tail[i], s3.tail[i]);
            }
        }

        [TestMethod]
        public void GetIntString_BasicallyWorking()
        {
            Snake s = new Snake(world, 0, heuristicFunc, true);
            var lstGen = s.Children;
            s = (Snake)lstGen.Last.Value;
            Assert.AreEqual("0-1", s.GetIntString());
        }
    }
}
