using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;

namespace Grid.Domain
{
    public class AlternateStepsHeuristic : IGridHeuristic
    {
        public int Calc_H(World w, GridSearchNode gridNode)
        {
            Queue<Location> open = new Queue<Location>();
            HashSet<Location> closed = new HashSet<Location>();
            open.Enqueue(w.Goal);
            bool goalReachableFromHead = false;
            int g = 0;
            int odd = 0;
            int even = 0;
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
                    if (IsEvenLocation(current))
                    {
                        even++;
                    }
                    else
                    {
                        odd++;
                    }
                }
            }

            bool FirstStepEven = !IsEvenLocation(gridNode.HeadLocation);
            bool GoalEven = IsEvenLocation(w.Goal);
            int h = 0;

            if (goalReachableFromHead)
            {
                if (FirstStepEven != GoalEven)   // This case is like classic Domino problem: [Head-W]->[FirstStep-B]->[W]->[B]->[Goal-W]
                {
                    h = Math.Min(odd, even) * 2;
                }
                else    // This case FIRST STEP AND GOAL ARE EQUALS: [Head-W]->[FirstStep-B]->[W]->[Goal-B]
                {
                    if (GoalEven)
                    {
                        if (even > odd)
                        {
                            h = odd * 2 + 1;
                        }
                        else
                        {
                            h = even * 2 - 1;
                        }
                    }
                    else //Goal is odd
                    {
                        if (odd > even)
                        {
                            h = even * 2 + 1;
                        }
                        else
                        {
                            h = odd * 2 - 1;
                        }
                    }
                }
            }

            return h;
        }

        public bool IsEvenLocation(Location loc)
        {
            return ((loc.X + loc.Y) % 2 == 0);
        }

        public string GetName()
        {
            return "AlternateStepsHeuristic";
        }
    }
}