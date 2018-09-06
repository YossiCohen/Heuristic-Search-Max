using Microsoft.VisualStudio.TestTools.UnitTesting;
using Grid;

namespace GridTest
{
    [TestClass]
    public class CpuCyclesCountTest
    {
        [TestMethod]
        public void Ignore_Sleep1()
        {
            ulong start, end;
            start = NativeMethods.GetThreadCycles();
            System.Threading.Thread.Sleep(1000);
            end = NativeMethods.GetThreadCycles();
            ulong cycles = end - start;
            Assert.IsTrue(cycles < 200000);
        }

        [TestMethod]
        public void Ignore_Sleep2()
        {
            ulong start, end;
            start = NativeMethods.GetThreadCycles();
            System.Threading.Thread.Sleep(3000);
            end = NativeMethods.GetThreadCycles();
            ulong cycles = end - start;
            Assert.IsTrue(cycles < 200000);
        }
    }
}
