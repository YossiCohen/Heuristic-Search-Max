namespace Grid.Domain
{
    public interface IGridHeuristic
    {
        int calc_h(World w, GridSearchNode gridNode);
    }
}
