using System;
using System.Collections.Generic;
using MaSib;
using MaSib.Algorithms;
using MaSib.Domain.SIB;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace MaSibTest
{
    [TestClass]
    public class BoxTest
    {
        //Public void MethodUnderTest_Scenario_ExpectedResult

        // these are needed on every test
        World world;

        private ISnakeHeuristic heuristicForSnakes;
        private IBoxHeuristic heuristicForBox;


        [TestInitialize]
        public void TestInitialize()
        {
            world = new World(7,2,3);
            heuristicForSnakes = new SnakeNoneHeuristic();
            heuristicForBox = new BoxNoneHeuristic();
        }

        [TestMethod]
        public void Create_NewBoxWithSnakes_CountsTwoSnakes()
        {
            int[] snakeHeads = new int[] {0, 63};
            BoxCartesian b = new BoxCartesian(world, snakeHeads, heuristicForBox, heuristicForSnakes);
            Assert.AreEqual(2, b.NumberOfSnake);
        }

        [TestMethod]
        public void Generate_BoxGenerateSnakes_SomeValidSnakesAreCreated()
        {
            int[] snakeHeads = new int[] { 0, 127 };
            BoxCartesian b = new BoxCartesian(world, snakeHeads, heuristicForBox, heuristicForSnakes);
            LinkedList<INode> boxes = b.Children;
            Assert.AreEqual(7, boxes.Count);
        }

        [TestMethod]
        public void Generate_BoxGenerateFourSnakes_SomeValidSnakesAreCreated()
        {
            int[] snakeHeads = new int[] { 0, 63, 15, 27 };
            BoxCartesian b = new BoxCartesian(world, snakeHeads, heuristicForBox, heuristicForSnakes);
            LinkedList<INode> boxes = b.Children;
            Assert.AreEqual(4, boxes.Count);
        }

        [TestMethod]
        public void Generate_BoxGenerateTwoSnakesBadSpread_PruneAll()
        {
            int[] snakeHeads = new int[] { 0, 3};
            BoxCartesian b = new BoxCartesian(world, snakeHeads, heuristicForBox, heuristicForSnakes);
            LinkedList<INode> boxes = b.Children;
            Assert.AreEqual(0, boxes.Count);
        }
    }
}
