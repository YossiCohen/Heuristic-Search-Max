namespace MaxSearchAlg
{
    public class NoPrunning : IPrunningMethod
    {
        public bool ShouldPrune(INode node)
        {
            return false;
        }

        public string GetName()
        {
            return "A None_Pr";
        }

        public void MemFlush()
        {

        }
    }
}