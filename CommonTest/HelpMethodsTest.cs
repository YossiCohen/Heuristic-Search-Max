using System;
using System.Collections;
using Commons;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommonTest
{
    [TestClass]
    public class HelpMethodsTest
    {
        [TestMethod]
        public void GetCardinality_CountShortArray_Returns_2()
        {
            BitArray a = new BitArray(new Boolean[]{true, true, false, false});
            Assert.AreEqual(2,HelpMethods.GetCardinality(a));
        }
        [TestMethod]
        public void GetCardinality_CountMediumArray_Returns_30()
        {
            BitArray a = new BitArray(new Boolean[] { true, true, false, false, true, true, false, false, true, true, false, false, true, true, false, false, true, true, false, false, true, true, false, false, true, true, false, false, true, true, false, false, true, true, false, false, true, true, false, false, true, true, false, false, true, true, false, false, true, true, false, false, true, true, false, false, true, true, false, false });
            Assert.AreEqual(30, HelpMethods.GetCardinality(a));
        }
        [TestMethod]
        public void GetCardinality_CountEmptyArray_Returns_0()
        {
            BitArray a = new BitArray(new Boolean[] { });
            Assert.AreEqual(0, HelpMethods.GetCardinality(a));
        }
    }
}
