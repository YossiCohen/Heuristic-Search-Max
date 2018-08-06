using System.Collections.Generic;

namespace Grid.Domain
{
    public class BiconnectedComponentsHeuristic : IGridHeuristic
    {
        public int Calc_H(World w, GridSearchNode gridNode)
        {
            var bcc = new BiconnectedComponents(w, gridNode);
            if (!bcc.LinearLocationWasVisitedDuringBuild(w.Goal))
            {
                return 0; //Goal not reachable
            }
            var valid = bcc.GetValidPlacesForMaxPath(gridNode.HeadLocation, w.Goal);
            int validCount = 0;
            foreach (var linearLocation in valid)
            {
                if (linearLocation) validCount++;
            }
            if (validCount>0) validCount--; //Minus 1 because we are counting the head location too
            return validCount; //Minus 1 because we are counting the head location too
        }
        
        public string GetName()
        {
            return "BiconnectedComponentsHeuristic";
        }
    }
}