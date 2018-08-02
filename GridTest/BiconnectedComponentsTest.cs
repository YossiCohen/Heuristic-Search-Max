using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Grid.Domain;

namespace GridTest
{
    [TestClass]
    public class BiconnectedComponentsTest
    {
        private static World _basicWorldV1;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            var _basicBCC_V1 = File.ReadAllText(@"..\..\Grid_5x5BiconnectedComponentsHeuristicV1.grd");
            World _basicWorldV1 = new World(_basicBCC_V1, new NoneHeuristic());
        }

        [TestMethod]
        public void Constructor_ReturnsValue_NotNull()
        {
            var _biconnectedComponents = new BiconnectedComponents(_basicWorldV1);
            Assert.IsNotNull(_biconnectedComponents);
        }


    }
}
