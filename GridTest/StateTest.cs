
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Grid.Domain;

namespace GridTest
{
    [TestClass]
    public class StateTest
    {
        private static World _basicWorld;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _basicWorld = new World(File.ReadAllText(@"..\..\Grid-10-6-5-4-0.grd"));
        }

        [TestMethod]
        public void Constructor_CreatesState_Created()
        {
            State s = new State(_basicWorld, _basicWorld.Start);
            Assert.IsNotNull(s);
        }
    }
}
