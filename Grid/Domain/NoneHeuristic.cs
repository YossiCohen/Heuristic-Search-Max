namespace Grid.Domain
{
    public class NoneHeuristic : IGridHeuristic
    {
        public int calc_h(World w, GridSearchNode gridNode)
        {
            return w.Width * w.Height;
        }

    }
}