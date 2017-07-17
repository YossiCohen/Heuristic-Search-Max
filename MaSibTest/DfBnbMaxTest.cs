using System;
using System.Collections.Generic;
using MaSib;
using MaSib.Algorithms;
using MaSib.Domain.SIB;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace MaSibTest
{
    [TestClass]
    public class DfBnbMaxTest
    {
        //Public void MethodUnderTest_Scenario_ExpectedResult

        [TestMethod]
        public void Create_NewAStarSnake()
        {
            World w = new World(4,2);
            var heuristicFunc = new SnakeNoneHeuristic();
            Snake snake = new Snake(w, 0, heuristicFunc);
            DfBnbMax astar = new DfBnbMax(snake);
            Assert.IsNotNull(astar);
        }

        [TestMethod]
        public void Run_NewAStarSnake()
        {
            World w = new World(5, 3);
            var heuristicFunc = new SnakeNoneHeuristic();
            Snake snake = new Snake(w, 0, heuristicFunc);
            DfBnbMax dfBnbMax = new DfBnbMax(snake);
            dfBnbMax.Run(1);
            var maxGoal = dfBnbMax.GetMaxGoal();
            Assert.IsNotNull(maxGoal);
        }

        [TestMethod]
        public void Run_NewAStarBox()
        {
            World w = new World(7, 2, 3);
            var heuristicFunc = new SnakeNoneHeuristic();
            int[] snakeHeads = new int[] { 0, 127 };
            BoxCartez b = new BoxCartez(w, snakeHeads,new BoxNoneHeuristic(), heuristicFunc);
            DfBnbMax dfBnbMax = new DfBnbMax(b);
            dfBnbMax.Run(1);
            var maxGoal = dfBnbMax.GetMaxGoal();
            Assert.IsNotNull(maxGoal);
        }

    }
}
