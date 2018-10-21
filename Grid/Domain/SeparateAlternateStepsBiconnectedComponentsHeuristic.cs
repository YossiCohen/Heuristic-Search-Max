using System.Collections;
using System.Linq;

namespace Grid.Domain
{
    public class SeparateAlternateStepsBiconnectedComponentsHeuristic : IGridHeuristic
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
            var bct = bcc.GetMaxPathBlockCutTree(gridNode.HeadLocation, w.Goal);
            //instead of g we will count odd and even separatly

            
            int h = 0;
            for (int i = 0; i + 1 < bct.Count; i += 2)
            {
                int head = bct.ElementAt(i)[0];
                var firstStepEven = !IsEvenLocation(w, head);
                var goalEven = IsEvenLocation(w, bct.ElementAt(i + 2)[0]);

                int odd = 0;
                int even = 0;
                var currBlk = bct.ElementAt(i+1);
                for (int j = 0; j < currBlk.Length; j++)
                {
                    if (currBlk[j] == head) continue;

                    if (IsEvenLocation(w, currBlk[j]))
                    {
                        even++;
                    }
                    else
                    {
                        odd++;
                    }
                }

                h += AlternateStepsHeuristic.CalculateAlternateStepHeuristic(firstStepEven, goalEven, odd, even);
            }

            if (gridNode is RsdGridSearchNode)
            {
                ((RsdGridSearchNode) gridNode).Reachable = new BitArray(w.LinearSize);
                for (int i = 0; i + 1 < bct.Count; i += 2)
                {
                    var currBlk = bct.ElementAt(i + 1);
                    for (int j = 0; j < currBlk.Length; j++)
                    {
                        ((RsdGridSearchNode) gridNode).Reachable[currBlk[j]] = true;
                    }
                }
            }

            return h;//AlternateStepsHeuristic.CalculateAlternateStepHeuristic(firstStepEven, goalEven, odd, even);
        }

        public bool IsEvenLocation(World w, int linearLocation)
        {
            int x = linearLocation % w.Width;
            int y = linearLocation / w.Width;
            return (x + y) % 2 == 0;
        }

        public string GetName()
        {
            return "SeparateAlternateStepsBiconnectedComponentsHeuristic";
        }
    }
}