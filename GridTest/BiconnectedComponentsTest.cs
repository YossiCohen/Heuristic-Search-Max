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
            /*
             * |---|---|---|---|---|
             * | 0 | 1 | 2 | 3 | 4 |
             * #############---|---| 
             * # 5 # 6 # 7 # 8 | 9 |
             * #############---|---| 
             * | 10| 11# 12# 13| 14|
             * |---|---#####---|---| 
             * | 15| 16# 17# 18| 19|
             * |---|---#####---|---| 
             * | 20| 21| 22| 23| 24|
             * |---|---|---|---|---| 
             * 
             */
            _basicWorldV1 = new World(_basicBCC_V1, new NoneHeuristic());
        }

        [TestMethod]
        public void Constructor_ReturnsValue_NotNull()
        {
            var _biconnectedComponents = new BiconnectedComponents(_basicWorldV1);
            Assert.IsNotNull(_biconnectedComponents);
        }

        [TestMethod]
        public void BlocksCount_ReturnsValue_7()
        {
            var _biconnectedComponents = new BiconnectedComponents(_basicWorldV1);
            Assert.AreEqual(7,_biconnectedComponents.Blocks.Count);
        }

        [TestMethod]
        public void CutPoints_AreAllValid_Count6()
        {
            var _biconnectedComponents = new BiconnectedComponents(_basicWorldV1);
            Assert.AreEqual(6,_biconnectedComponents.CutPoints.Count);
            Assert.IsTrue(_biconnectedComponents.CutPoints.Contains(1));
            Assert.IsTrue(_biconnectedComponents.CutPoints.Contains(2));
            Assert.IsTrue(_biconnectedComponents.CutPoints.Contains(3));
            Assert.IsTrue(_biconnectedComponents.CutPoints.Contains(23));
            Assert.IsTrue(_biconnectedComponents.CutPoints.Contains(22));
            Assert.IsTrue(_biconnectedComponents.CutPoints.Contains(21));
        }

        [TestMethod]
        public void GetValidPlacesForMaxPath_SmallPath_AreAllValid()
        {
            var _biconnectedComponents = new BiconnectedComponents(_basicWorldV1);
            var valid = _biconnectedComponents.GetValidPlacesForMaxPath(23, 21);
            Assert.IsFalse(valid[0]);
            Assert.IsFalse(valid[1]);
            Assert.IsFalse(valid[2]);
            Assert.IsFalse(valid[3]);
            Assert.IsFalse(valid[4]);
            Assert.IsFalse(valid[5]);
            Assert.IsFalse(valid[6]);
            Assert.IsFalse(valid[7]);
            Assert.IsFalse(valid[8]);
            Assert.IsFalse(valid[9]);
            Assert.IsFalse(valid[10]);
            Assert.IsFalse(valid[11]);
            Assert.IsFalse(valid[12]);
            Assert.IsFalse(valid[13]);
            Assert.IsFalse(valid[14]);
            Assert.IsFalse(valid[15]);
            Assert.IsFalse(valid[16]);
            Assert.IsFalse(valid[17]);
            Assert.IsFalse(valid[18]);
            Assert.IsFalse(valid[19]);
            Assert.IsFalse(valid[20]);
            Assert.IsTrue(valid[21]);
            Assert.IsTrue(valid[22]);
            Assert.IsTrue(valid[23]);
            Assert.IsFalse(valid[24]);
        }


        [TestMethod]
        public void GetValidPlacesForMaxPath_SameBlock_AreAllValid()
        {
            var _biconnectedComponents = new BiconnectedComponents(_basicWorldV1);
            var valid = _biconnectedComponents.GetValidPlacesForMaxPath(9, 18);
            Assert.IsFalse(valid[0]);
            Assert.IsFalse(valid[1]);
            Assert.IsFalse(valid[2]);
            Assert.IsTrue(valid[3]);
            Assert.IsTrue(valid[4]);
            Assert.IsFalse(valid[5]);
            Assert.IsFalse(valid[6]);
            Assert.IsFalse(valid[7]);
            Assert.IsTrue(valid[8]);
            Assert.IsTrue(valid[9]);
            Assert.IsFalse(valid[10]);
            Assert.IsFalse(valid[11]);
            Assert.IsFalse(valid[12]);
            Assert.IsTrue(valid[13]);
            Assert.IsTrue(valid[14]);
            Assert.IsFalse(valid[15]);
            Assert.IsFalse(valid[16]);
            Assert.IsFalse(valid[17]);
            Assert.IsTrue(valid[18]);
            Assert.IsTrue(valid[19]);
            Assert.IsFalse(valid[20]);
            Assert.IsFalse(valid[21]);
            Assert.IsFalse(valid[22]);
            Assert.IsTrue(valid[23]);
            Assert.IsTrue(valid[24]);
        }
        
        [TestMethod]
        public void GetValidPlacesForMaxPath_AllBlocks_AreAllValid()
        {
            var _biconnectedComponents = new BiconnectedComponents(_basicWorldV1);
            var valid = _biconnectedComponents.GetValidPlacesForMaxPath(0, 11);
            Assert.IsTrue(valid[0]);
            Assert.IsTrue(valid[1]);
            Assert.IsTrue(valid[2]);
            Assert.IsTrue(valid[3]);
            Assert.IsTrue(valid[4]);
            Assert.IsFalse(valid[5]);
            Assert.IsFalse(valid[6]);
            Assert.IsFalse(valid[7]);
            Assert.IsTrue(valid[8]);
            Assert.IsTrue(valid[9]);
            Assert.IsTrue(valid[10]);
            Assert.IsTrue(valid[11]);
            Assert.IsFalse(valid[12]);
            Assert.IsTrue(valid[13]);
            Assert.IsTrue(valid[14]);
            Assert.IsTrue(valid[15]);
            Assert.IsTrue(valid[16]);
            Assert.IsFalse(valid[17]);
            Assert.IsTrue(valid[18]);
            Assert.IsTrue(valid[19]);
            Assert.IsTrue(valid[20]);
            Assert.IsTrue(valid[21]);
            Assert.IsTrue(valid[22]);
            Assert.IsTrue(valid[23]);
            Assert.IsTrue(valid[24]);
        }


        [TestMethod]
        public void GetValidPlacesForMaxPath_AlmostAllBlocks_AreAllValid()
        {
            var _biconnectedComponents = new BiconnectedComponents(_basicWorldV1);
            var valid = _biconnectedComponents.GetValidPlacesForMaxPath(1, 11);
            Assert.IsFalse(valid[0]);
            Assert.IsTrue(valid[1]);
            Assert.IsTrue(valid[2]);
            Assert.IsTrue(valid[3]);
            Assert.IsTrue(valid[4]);
            Assert.IsFalse(valid[5]);
            Assert.IsFalse(valid[6]);
            Assert.IsFalse(valid[7]);
            Assert.IsTrue(valid[8]);
            Assert.IsTrue(valid[9]);
            Assert.IsTrue(valid[10]);
            Assert.IsTrue(valid[11]);
            Assert.IsFalse(valid[12]);
            Assert.IsTrue(valid[13]);
            Assert.IsTrue(valid[14]);
            Assert.IsTrue(valid[15]);
            Assert.IsTrue(valid[16]);
            Assert.IsFalse(valid[17]);
            Assert.IsTrue(valid[18]);
            Assert.IsTrue(valid[19]);
            Assert.IsTrue(valid[20]);
            Assert.IsTrue(valid[21]);
            Assert.IsTrue(valid[22]);
            Assert.IsTrue(valid[23]);
            Assert.IsTrue(valid[24]);
        }
    }
}
