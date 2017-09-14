using System;
using System.Collections.Generic;
using Facet.Combinatorics;
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
            Assert.AreEqual(15, boxes.Count);
        }

        [TestMethod]
        public void Generate_BoxGenerateFourSnakes_SomeValidSnakesAreCreated()
        {
            int[] snakeHeads = new int[] { 0, 63, 15, 27 };
            BoxCartesian b = new BoxCartesian(world, snakeHeads, heuristicForBox, heuristicForSnakes);
            LinkedList<INode> boxes = b.Children;
            Assert.AreEqual(28, boxes.Count);
        }

        [TestMethod]
        public void Generate_BoxGenerateTwoSnakesBadSpread_PruneAll()
        {
            int[] snakeHeads = new int[] { 0, 3};
            BoxCartesian b = new BoxCartesian(world, snakeHeads, heuristicForBox, heuristicForSnakes);
            LinkedList<INode> boxes = b.Children;
            Assert.AreEqual(0, boxes.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_NewBoxVirtualNode()
        {
            BoxVirtualNode<BoxOD> box = new BoxVirtualNode<BoxOD>(world, 4, heuristicForBox, heuristicForSnakes);
            Assert.IsNotNull(box);
            BoxVirtualNode<BoxCartesian> box2 = new BoxVirtualNode<BoxCartesian>(world, 4, heuristicForBox, heuristicForSnakes);
            Assert.IsNotNull(box2);
            BoxVirtualNode<String> box3 = new BoxVirtualNode<String>(world, 4, heuristicForBox, heuristicForSnakes);
        }

        [TestMethod]
        public void BoxVirtualNode_Children_Valid()
        {
            BoxVirtualNode<BoxOD> box = new BoxVirtualNode<BoxOD>(world, 4, heuristicForBox, heuristicForSnakes);
            Assert.IsNotNull(box);
            var childs = box.Children;
            Assert.AreEqual(333375, childs.Count);
        }

        [TestMethod]
        public void BoxVirtualNode_Children_Valid2()
        {
            World w = new World(6, 2, 3);
            BoxVirtualNode<BoxOD> box = new BoxVirtualNode<BoxOD>(w, 2, heuristicForBox, heuristicForSnakes);
            Assert.IsNotNull(box);
            var childs = box.Children;
            Assert.AreEqual(63, childs.Count);
        }


        [TestMethod]
        public void Combinatorics_sanitiy()
        {
            var integers = new List<int> { 1, 2, 3, 4, 5 };

            var c = new Combinations<int>(integers, 3);

            foreach (var v in c)
            {
                System.Diagnostics.Debug.WriteLine(string.Join(",", v));
            }
            foreach (IList<int> cc in c)
            {
                Console.WriteLine(String.Format("{{{0} {1} {2}}}", cc[0], cc[1], cc[2]));
            }
            Assert.AreEqual(10, c.Count);
        }
    }
}
