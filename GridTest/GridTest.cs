using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Grid.Domain;

namespace UnitTestProject1
{
    [TestClass]
    public class GridTest
    {
        [TestMethod]
        public void Create_World()
        {
            World g = new World("aa");
            Assert.IsNotNull(g);
        }
    }
}
