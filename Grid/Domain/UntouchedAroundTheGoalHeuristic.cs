using System.Collections.Generic;

namespace Grid.Domain
{
    public class UntouchedAroundTheGoalHeuristic : IGridHeuristic
    {
        public int Calc_H(World w, GridSearchNode gridNode)
        {
            Queue<Location> open = new Queue<Location>();
            HashSet<Location> closed = new HashSet<Location>();
            open.Enqueue(w.Goal);
            bool goalReachableFromHead = false;
            int g = 0;
            while (open.Count > 0)
            {
                var current = open.Dequeue();
                if (closed.Contains(current) || current.X <0 || current.X>=w.Width || current.Y<0 || current.Y>=w.Height)
                { //Already seen or Out of the frame
                    continue;
                }
                closed.Add(current);
                if (current.Equals(gridNode.HeadLocation))
                {
                    goalReachableFromHead = true;
                }
                if (!w.IsBlocked(current) && !gridNode.IsVisited(current))
                {
                    open.Enqueue(current.GetMovedLocation(MoveDirection.Up));
                    open.Enqueue(current.GetMovedLocation(MoveDirection.Down));
                    open.Enqueue(current.GetMovedLocation(MoveDirection.Left));
                    open.Enqueue(current.GetMovedLocation(MoveDirection.Right));
                    g++;
                }

            }
            return goalReachableFromHead ? g:0;
        }
        public string GetName()
        {
            return "B Reachable_H";
        }
    }
}