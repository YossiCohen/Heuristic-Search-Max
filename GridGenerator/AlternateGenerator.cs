using System;
using System.Collections.Generic;

namespace GridGenerator
{
    class AlternateGenerator : GridBase
    {
        private int _basicBlockedOdd;
        private int _basicBlockedEven;
        private bool _startGoalCorners;

        private Random _r = new Random();

        public AlternateGenerator(Dictionary<string, string> splitedArgs)
        {
            GridSizeX = int.Parse(splitedArgs["alternate-width"]);
            GridSizeY = int.Parse(splitedArgs["alternate-hight"]);
            _basicBlockedOdd = int.Parse(splitedArgs["alternate-blocked-odd"]);
            _basicBlockedEven = int.Parse(splitedArgs["alternate-blocked-even"]);
            _startGoalCorners = bool.Parse(splitedArgs["alternate-corners"]);
        }

        public override bool ValidArgs()
        {
            if (GridSizeX <= 1 || GridSizeY <= 1)
            {
                Console.WriteLine("Grid size must be greater than 1 on both dimensions");
                return false;
            }
            if (_basicBlockedOdd <= 1 || _basicBlockedEven <= 1)
            {
                Console.WriteLine("Both block parameters must be at least 1");
                return false;
            }
            return true;
        }

        public override void AddBlockedLocationsStartAndGoal()
        {
            //REMEMBER: Start location is a Blocked location!
            int numOfOddBarriers = _basicBlockedOdd;
            int numOfEvenBarriers = _basicBlockedEven;

            //add start and goal locations
            if (_startGoalCorners)
            {
                Board[0, 0] = 2;
                Board[GridSizeY - 1, GridSizeX - 1] = 3;
                numOfEvenBarriers--;
            }
            else
            {
                int x, y;
                //random start
                y = _r.Next(GridSizeY);
                x = _r.Next(GridSizeX);
                Board[y, x] = 2;
                if ((y + x) % 2 == 0)
                {
                    numOfEvenBarriers--;
                }
                else
                {
                    numOfOddBarriers--;
                }

                //random goal != start
                do
                {
                    x = _r.Next(GridSizeX);
                    y = _r.Next(GridSizeY);
                } while (Board[y, x] == 2);
                Board[y, x] = 3;
            }
            
            //Adding Odd barriers
            while (numOfOddBarriers > 0)
            {
                int x = _r.Next(GridSizeX);
                int y = _r.Next(GridSizeY);
                if ((x + y) % 2 == 1 && Board[y, x] == 0)
                {
                    Board[y, x] = 1;
                    numOfOddBarriers--;
                }
            }

            //Adding Even barriers
            while (numOfEvenBarriers > 0)
            {
                int x = _r.Next(GridSizeX);
                int y = _r.Next(GridSizeY);
                if ((x + y) % 2 == 0 && Board[y, x] == 0)
                {
                    Board[y, x] = 1;
                    numOfEvenBarriers--;
                }
            }
        }

        public override string GetFileName(int id, int setSize)
        {
            string outFileName = "" + id + "-of-" + setSize + 
                                 "-Size[" + GridSizeX + "x" + GridSizeY +
                                 "]-EvenBlocked[" + _basicBlockedEven + "]-OddBlocked[" + _basicBlockedOdd + "]-Corners[" + _startGoalCorners+ "].grd";
            return outFileName;
        }

    }
}
