using System.Collections;

namespace Grid.Domain
{
    public class AlternateStepsBiconnectedComponentsHeuristic : IGridHeuristic
    {
        public int Calc_H(World w, GridSearchNode gridNode)
        {
            var bcc = new BiconnectedComponents(w, gridNode);
            if (!bcc.LinearLocationWasVisitedDuringBuild(w.Goal))
            {
                if (gridNode is RsdGridSearchNode)
                {
                    ((RsdGridSearchNode)gridNode).Reachable = new BitArray(w.LinearSize);
                }
                return 0; //Goal not reachable
            }
            var valid = bcc.GetValidPlacesForMaxPath(gridNode.HeadLocation, w.Goal);
            //instead of g we will count odd and even separatly
            int odd = 0;
            int even = 0;

            int head = gridNode.HeadLocation.GetLinearLocationRepresentation(w);
            for (int i = 0; i < valid.Length; i++)
            {
                if (valid[i])
                {
                    if (i == head) continue;

                    if (IsEvenLocation(w, i))
                    {
                        even++;
                    }
                    else
                    {
                        odd++;
                    }
                }
            }
            
            if (gridNode is RsdGridSearchNode)
            {
                ((RsdGridSearchNode)gridNode).Reachable = new BitArray(valid); //TODO: head location valid?
            }


            bool firstStepEven = !IsEvenLocation(w, head);
            bool goalEven = IsEvenLocation(w, w.Goal.GetLinearLocationRepresentation(w));

            return AlternateStepsHeuristic.CalculateAlternateStepHeuristic(firstStepEven, goalEven, odd, even);
        }

        public bool IsEvenLocation(World w, int linearLocation)
        {
            int x = linearLocation % w.Width;
            int y = linearLocation / w.Width;
            return (x + y) % 2 == 0;
        }

        public string GetName()
        {
            return "E Alt_BCC_H";
        }
    }
}