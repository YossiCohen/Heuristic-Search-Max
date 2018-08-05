using System.Collections.Generic;

namespace Grid.Domain
{
    public class BiconnectedComponentsHeuristic : IGridHeuristic
    {
        public int Calc_H(World w, GridSearchNode gridNode)
        {
            return 0;
        }

        public string GetName()
        {
            return "BiconnectedComponentsHeuristic";
        }
    }
}