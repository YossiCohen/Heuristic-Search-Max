namespace MaxSearchAlg
{
    public interface IPrunningMethod
    {
        bool ShouldPrune(INode node);
        string GetName();
    }
}