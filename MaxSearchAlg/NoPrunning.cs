namespace MaxSearchAlg
{
    public class NoPrunning : IPrunningMethod
    {
        public bool ShouldPrune(INode node)
        {
            return false;
        }
    }
}