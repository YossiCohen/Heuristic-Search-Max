using System;
using System.Collections.Generic;
using MaSib;
using MaSib.Domain.SIB;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace MaSibTest
{
    [TestClass]
    public class HeuristicTest
    {
        //Public void MethodUnderTest_Scenario_ExpectedResult

        // these are needed on every test
        World world;


        [TestInitialize]
        public void TestInitialize()
        {
            world = new World(3,2);
        }

        [TestMethod]
        public void Calculate_NoneHeuristic()
        {
            SnakeNoneHeuristic mh = new SnakeNoneHeuristic();
            Snake s = new Snake(world, 0, mh, true);
            Assert.AreEqual(1, s.g);
            Assert.AreEqual(world.MaxPlacesInDimention, s.h);
            Assert.AreEqual(world.MaxPlacesInDimention + 1, s.f);
        }

        [TestMethod]
        public void Calculate_RightLegalHeuristic()
        {
            SnakeLegalHeuristic lh = new SnakeLegalHeuristic();
            Snake s = new Snake(world, 0, lh, true);
            Assert.AreEqual(1, s.g);
            Assert.AreEqual(6, s.h);
            Assert.AreEqual(7, s.f);
            var lstGen = s.Children;
            Assert.AreEqual(2, lstGen.Last.Value.g);
            Assert.AreEqual(4, lstGen.Last.Value.h);
            Assert.AreEqual(6, lstGen.Last.Value.f);
            lstGen = lstGen.Last.Value.Children;
            Assert.AreEqual(3, lstGen.Last.Value.g);
            Assert.AreEqual(3, lstGen.Last.Value.h);
            Assert.AreEqual(6, lstGen.Last.Value.f);
        }

        [TestMethod]
        public void Calculate_RightReachableHeuristic()
        {
            SnakeReachableHeuristic lh = new SnakeReachableHeuristic();
            Snake s = new Snake(world, 0, lh, true);
            Assert.AreEqual(1, s.g);
            Assert.AreEqual(7, s.h);
            Assert.AreEqual(8, s.f);
            var lstGen = s.Children;
            Assert.AreEqual(2, lstGen.Last.Value.g);
            Assert.AreEqual(7, lstGen.Last.Value.h);
            Assert.AreEqual(9, lstGen.Last.Value.f);
            lstGen = lstGen.Last.Value.Children;
            Assert.AreEqual(3, lstGen.Last.Value.g);
            Assert.AreEqual(3, lstGen.Last.Value.h);
            Assert.AreEqual(6, lstGen.Last.Value.f);
        }


        [TestMethod]
        public void Calculate_RightBoxSnakesSumHeuristic()
        {
            World w = new World(7, 2, 3);
            int[] snakeHeads = new int[] { 0, 127 };
            BoxCartez b = new BoxCartez(w, snakeHeads, new BoxSnakesSumHeuristic(), new SnakeLegalHeuristic());
            Assert.AreEqual(2, b.g);
            Assert.AreEqual(128, b.h);
            Assert.AreEqual(130, b.f);
        }
    }

}
