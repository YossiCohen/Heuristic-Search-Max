namespace Grid.Domain
{
    public interface IGridHeuristic
    {
        int Calc_H(World w, GridSearchNode gridNode);
        int Calc_Life_H(World w, GridSearchNode gridNode);
        string GetName();
    }
}

