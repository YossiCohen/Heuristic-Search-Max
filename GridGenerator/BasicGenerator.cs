using System;
using System.Collections.Generic;

namespace GridGenerator
{
    class BasicGenerator : GridBase
    {
        private int _basicBlocked;
        private bool _startGoalCorners;

        private Random _r = new Random();

        public BasicGenerator(Dictionary<string, string> splitedArgs)
        {
            GridSizeX = int.Parse(splitedArgs["basic-width"]);
            GridSizeY = int.Parse(splitedArgs["basic-hight"]);
            _basicBlocked = int.Parse(splitedArgs["basic-blocked"]);
            _startGoalCorners = bool.Parse(splitedArgs["basic-corners"]);
        }

        public override bool ValidArgs()
        {
            if (GridSizeX <= 1 || GridSizeY <= 1)
            {
                Console.WriteLine("Grid size must be greater than 1 on both dimensions");
                return false;
            }
            return true;
        }

        public override void AddBlockedLocationsStartAndGoal()
        {
            //add start and goal locations
            if (_startGoalCorners)
            {
                Board[0, 0] = 2;
                Board[GridSizeY - 1, GridSizeX - 1] = 3;
            }
            else
            {
                //random start
                Board[_r.Next(GridSizeY), _r.Next(GridSizeX)] = 2;
                //random goal != start
                int x, y;
                do
                {
                    x = _r.Next(GridSizeX);
                    y = _r.Next(GridSizeY);
                } while (Board[y, x] == 2);
                Board[y, x] = 3;
            }
            
            //Adding barriers
            int numOfBarriers = _basicBlocked;
            while (numOfBarriers > 0)
            {
                int x = _r.Next(GridSizeX);
                int y = _r.Next(GridSizeY);
                if (Board[y, x] == 0)
                {
                    Board[y, x] = 1;
                    numOfBarriers--;
                }
            }
        }

        public override string GetFileName(int id, int setSize)
        {
            string outFileName = "" + id + "-of-" + setSize + 
                                 "-Size[" + GridSizeX + "x" + GridSizeY + 
                                 "]-BlockedCount[" + _basicBlocked+ "]-Corners[" + _startGoalCorners+ "].grd";
            return outFileName;
        }

    }
}
