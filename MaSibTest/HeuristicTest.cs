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
            Assert.AreEqual(0, s.g);
            Assert.AreEqual(world.MaxPlacesInDimention, s.h);
            Assert.AreEqual(world.MaxPlacesInDimention , s.f);
        }

        [TestMethod]
        public void Calculate_RightSnakeLegalHeuristic()
        {
            SnakeLegalHeuristic lh = new SnakeLegalHeuristic();
            Snake s = new Snake(world, 0, lh, true);
            Assert.AreEqual(0, s.g);
            Assert.AreEqual(6, s.h);
            Assert.AreEqual(6, s.f);
            var lstGen = s.Children;
            Assert.AreEqual(1, lstGen.Last.Value.g);
            Assert.AreEqual(4, lstGen.Last.Value.h);
            Assert.AreEqual(5, lstGen.Last.Value.f);
            lstGen = lstGen.Last.Value.Children;
            Assert.AreEqual(2, lstGen.Last.Value.g);
            Assert.AreEqual(3, lstGen.Last.Value.h);
            Assert.AreEqual(5, lstGen.Last.Value.f);
        }

        [TestMethod]
        public void Calculate_RightSnakeReachableHeuristic()
        {
            SnakeReachableHeuristic lh = new SnakeReachableHeuristic();
            Snake s = new Snake(world, 0, lh, true);
            Assert.AreEqual(0, s.g);
            Assert.AreEqual(7, s.h);
            Assert.AreEqual(7, s.f);
            var lstGen = s.Children;
            Assert.AreEqual(1, lstGen.Last.Value.g);
            Assert.AreEqual(7, lstGen.Last.Value.h);
            Assert.AreEqual(8, lstGen.Last.Value.f);
            lstGen = lstGen.Last.Value.Children;
            Assert.AreEqual(2, lstGen.Last.Value.g);
            Assert.AreEqual(3, lstGen.Last.Value.h);
            Assert.AreEqual(5, lstGen.Last.Value.f);
        }


        [TestMethod]
        public void Calculate_RightBoxSnakesSumHeuristic()
        {
            World w = new World(7, 2, 3);
            int[] snakeHeads = new int[] { 0, 127 };
            BoxCartesian b = new BoxCartesian(w, snakeHeads, new BoxSnakesSumHeuristic(), new SnakeLegalHeuristic());
            Assert.AreEqual(0, b.g);
            Assert.AreEqual(128, b.h);
            Assert.AreEqual(128, b.f);
            var lstGen = b.Children;
            Assert.AreEqual(1, lstGen.Last.Value.g);
            Assert.AreEqual(127, lstGen.Last.Value.h);
            Assert.AreEqual(128, lstGen.Last.Value.f);
            lstGen = lstGen.Last.Value.Children;
            Assert.AreEqual(2, lstGen.Last.Value.g);
            Assert.AreEqual(126, lstGen.Last.Value.h);
            Assert.AreEqual(128, lstGen.Last.Value.f);
        }

        [TestMethod]
        public void Calculate_RightBoxLegalHeuristic()
        {
            World w = new World(7, 2, 3);
            int[] snakeHeads = new int[] { 0, 127 };
            BoxCartesian b = new BoxCartesian(w, snakeHeads, new BoxLegalHeuristic(), new SnakeNoneHeuristic());
            Assert.AreEqual(0, b.g);
            Assert.AreEqual(114, b.h);
            Assert.AreEqual(114, b.f);
            var lstGen = b.Children;
            Assert.AreEqual(1, lstGen.Last.Value.g);
            Assert.AreEqual(108, lstGen.Last.Value.h);
            Assert.AreEqual(109, lstGen.Last.Value.f);
            lstGen = lstGen.First.Value.Children;
            Assert.AreEqual(3, lstGen.Last.Value.g);
            Assert.AreEqual(97, lstGen.Last.Value.h);
            Assert.AreEqual(100, lstGen.Last.Value.f);
            lstGen = lstGen.First.Value.Children;
            Assert.AreEqual(5, lstGen.Last.Value.g);
            Assert.AreEqual(87, lstGen.Last.Value.h);
            Assert.AreEqual(92, lstGen.Last.Value.f);
        }

        [TestMethod]
        public void Calculate_RightBoxReachableHeuristic()
        {
            World w = new World(7, 2, 3);
            int[] snakeHeads = new int[] { 0, 127 };
            BoxCartesian b = new BoxCartesian(w, snakeHeads, new BoxReachableHeuristic(), new SnakeNoneHeuristic());
            Assert.AreEqual(0, b.g);
            Assert.AreEqual(126, b.h);
            Assert.AreEqual(126, b.f);
            var lstGen = b.Children;
            Assert.AreEqual(1, lstGen.Last.Value.g);
            Assert.AreEqual(126, lstGen.Last.Value.h);
            Assert.AreEqual(127, lstGen.Last.Value.f);
            lstGen = lstGen.First.Value.Children;
            Assert.AreEqual(3, lstGen.Last.Value.g);
            Assert.AreEqual(118, lstGen.Last.Value.h);
            Assert.AreEqual(121, lstGen.Last.Value.f);
            lstGen = lstGen.First.Value.Children;
            Assert.AreEqual(5, lstGen.Last.Value.g);
            Assert.AreEqual(104, lstGen.Last.Value.h);
            Assert.AreEqual(109, lstGen.Last.Value.f);
        }
    }

}
