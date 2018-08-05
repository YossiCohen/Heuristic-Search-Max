namespace Grid.Domain
{
    public class NoneHeuristic : IGridHeuristic
    {
        public int Calc_H(World w, GridSearchNode gridNode)
        {
            return w.Width * w.Height;
        }
        public string GetName()
        {
            return "None";
        }
    }
}