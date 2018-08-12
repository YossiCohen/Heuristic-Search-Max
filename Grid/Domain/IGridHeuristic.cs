namespace Grid.Domain
{
    public interface IGridHeuristic
    {
        int Calc_H(World w, GridSearchNode gridNode);
        string GetName();
    }
}

