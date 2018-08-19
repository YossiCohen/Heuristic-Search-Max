using System.Collections;

namespace Grid.Domain
{
    public class BiconnectedComponentsHeuristic : IGridHeuristic
    {
        public int Calc_H(World w, GridSearchNode gridNode)
        {
            var bcc = new BiconnectedComponents(w, gridNode);
            if (!bcc.LinearLocationWasVisitedDuringBuild(w.Goal))
            {
                if (gridNode is RsdGridSearchNode)
                {
                    ((RsdGridSearchNode)gridNode).Reachable = new BitArray(w.LinearSize); //TODO: head location valid?
                }
                return 0; //Goal not reachable
            }
            var valid = bcc.GetValidPlacesForMaxPath(gridNode.HeadLocation, w.Goal);
            int validCount = 0;
            foreach (var linearLocation in valid)
            {
                if (linearLocation) validCount++;
            }
            if (validCount>0) validCount--; //Minus 1 because we are counting the head location too
            if (gridNode is RsdGridSearchNode)
            {
                ((RsdGridSearchNode)gridNode).Reachable = new BitArray(valid); //TODO: head location valid?
            }
            return validCount; //Minus 1 because we are counting the head location too
        }
        
        public string GetName()
        {
            return "BiconnectedComponentsHeuristic";
        }
    }
}