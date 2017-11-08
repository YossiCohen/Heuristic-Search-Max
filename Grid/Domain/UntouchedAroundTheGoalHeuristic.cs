using System.Collections.Generic;

namespace Grid.Domain
{
    public class UntouchedAroundTheGoalHeuristic : IGridHeuristic
    {
        public int calc_h(World w, GridSearchNode gridNode)
        {
            List<Location> openList = new List<Location>();
            openList.Add(gridNode.HeadLocation);
            while (openList.Count > 0)
            {
                
            }
            return -1;
        }

    }
}