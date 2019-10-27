using System;
using System.Collections;
using System.Collections.Generic;

namespace Grid.Domain
{
    public class AlternateStepsHeuristic : IGridHeuristic
    {
        public int Calc_H(World w, GridSearchNode gridNode)
        {
            if (gridNode is RsdGridSearchNode)
            {
                ((RsdGridSearchNode)gridNode).Reachable = new BitArray(w.LinearSize);
            }

            Queue<Location> open = new Queue<Location>();
            HashSet<Location> closed = new HashSet<Location>();
            open.Enqueue(w.Goal);
            bool goalReachableFromHead = false;
            //instead of g we will count odd and even separatly
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
                    if (gridNode is RsdGridSearchNode)
                    {
                        ((RsdGridSearchNode)gridNode).Reachable[current.Y * w.Width + current.X] = true;
                    }

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

            if (!goalReachableFromHead) return 0;

            bool firstStepEven = !IsEvenLocation(gridNode.HeadLocation);
            bool goalEven = IsEvenLocation(w.Goal);

            return CalculateAlternateStepHeuristic(firstStepEven, goalEven, odd, even);
        }

        internal static int CalculateAlternateStepHeuristic(bool firstStepEven, bool goalEven, int oddCount, int evenCount)
        {
            if (oddCount == 0 && evenCount == 0) return 0;
            if (firstStepEven != goalEven) // This case is like classic Domino problem: [Head-W]->[FirstStep-B]->[W]->[B]->[Goal-W]
            {
                return Math.Min(oddCount, evenCount) * 2;
            }
            else // This case FIRST STEP AND GOAL ARE EQUALS: [Head-W]->[FirstStep-B]->[W]->[Goal-B]
            {
                if (goalEven)
                {
                    if (evenCount > oddCount)
                    {
                        return oddCount * 2 + 1;
                    }
                    else
                    {
                        return evenCount * 2 - 1;
                    }
                }
                else //Goal is odd
                {
                    if (oddCount > evenCount)
                    {
                        return evenCount * 2 + 1;
                    }
                    else
                    {
                        return oddCount * 2 - 1;
                    }
                }
            }
        }

        public bool IsEvenLocation(Location loc)
        {
            return (loc.X + loc.Y) % 2 == 0;
        }

        public static int GetNumberOfEvenLocations(int width, int height)
        {
            return ((width + 1) / 2) * ((height + 1) / 2) + (width / 2) * (height / 2);
        }

        public static int GetNumberOfOddLocations(int width, int height)
        {
            return width * height - GetNumberOfEvenLocations(width, height);
        }

        public int Calc_Life_H(World w, GridSearchNode gridNode)
        {
            Queue<Location> open = new Queue<Location>();
            HashSet<Location> closed = new HashSet<Location>();
            open.Enqueue(w.Goal);
            bool goalReachableFromHead = false;
            //instead of g we will count odd and even separatly
            int[] odd = new int[w.Height];
            int[] even = new int[w.Height];
            int oddCount = 0;
            int evenCount = 0;

            while (open.Count > 0)
            {
                var current = open.Dequeue();
                if (closed.Contains(current) || current.X < 0 || current.X >= w.Width || current.Y < 0 || current.Y >= w.Height)
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

                    if (IsEvenLocation(current))
                    {
                        even[current.Y]++;
                        evenCount ++;
                    }
                    else
                    {
                        odd[current.Y]++;
                        oddCount ++;
                    }
                }
            }

            if (!goalReachableFromHead) return 0;

            bool firstStepEven = !IsEvenLocation(gridNode.HeadLocation);
            bool goalEven = IsEvenLocation(w.Goal);

            return CalculateAlternateStepHeuristicLife(firstStepEven, goalEven, odd, even, oddCount, evenCount);
        }

        internal static int CalculateAlternateStepHeuristicLife(bool firstStepEven, bool goalEven, int[] oddCountArr, int[] evenCountArr, int oddCount, int evenCount)
        {
            if (oddCount == 0 && evenCount == 0) return 0;

            if (firstStepEven != goalEven) // This case is like classic Domino problem: [Head-W]->[FirstStep-B]->[W]->[B]->[Goal-W]
            {
                var count = Math.Min(oddCount, evenCount);
                return GetHighest_N_ItemsLife(oddCountArr,count) + GetHighest_N_ItemsLife(evenCountArr, count);
            }
            else // This case FIRST STEP AND GOAL ARE EQUALS: [Head-W]->[FirstStep-B]->[W]->[Goal-B]
            {
                if (goalEven)
                {
                    if (evenCount > oddCount)
                    {
                        return GetHighest_N_ItemsLife(oddCountArr, oddCount) + GetHighest_N_ItemsLife(evenCountArr, oddCount + 1); //oddCount * 2 + 1;
                    }
                    else
                    {
                        return GetHighest_N_ItemsLife(oddCountArr, evenCount -1) + GetHighest_N_ItemsLife(evenCountArr, evenCount); //evenCount * 2 - 1;
                    }
                }
                else //Goal is odd
                {
                    if (oddCount > evenCount)
                    {
                        return GetHighest_N_ItemsLife(oddCountArr, evenCount + 1) + GetHighest_N_ItemsLife(evenCountArr, evenCount); //evenCount * 2 + 1;
                    }
                    else
                    {
                        return GetHighest_N_ItemsLife(oddCountArr, oddCount) + GetHighest_N_ItemsLife(evenCountArr, oddCount -1); // oddCount * 2 - 1;
                    }
                }
            }
        }

        private static int GetHighest_N_ItemsLife(int[] oddOrEvenCountArr, int numToCount)
        {
            int acc = 0;
            int i = oddOrEvenCountArr.Length - 1;
            while (i >= 0)
            {
                acc += oddOrEvenCountArr[i] * (i + 1);
                numToCount -= oddOrEvenCountArr[i];
                if (numToCount <= 0)
                {
                    acc += numToCount * i;
                    break;
                }
                i--;
            }
            return acc;
        }

        public string GetName()
        {
            return "C Alt_Reachable_H";
        }
    }
}