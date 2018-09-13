using System;
using System.Collections.Generic;

namespace GridGenerator
{
    class RoomsGenerator : GridBase
    {
        private int _num_rooms_x;
        private int _num_rooms_y;
        private int _room_size_x;
        private int _room_size_y;
        private int _wall_doors_x;
        private int _wall_doors_y;

        private double _door_open_p;
        private double _barier_p;
        private Random _r = new Random();

        public RoomsGenerator(Dictionary<string, string> splitedArgs)
        {
            _num_rooms_x = int.Parse(splitedArgs["rooms-num-x"]);
            _num_rooms_y = int.Parse(splitedArgs["rooms-num-y"]);
            _room_size_x = int.Parse(splitedArgs["rooms-size-x"]);
            _room_size_y = int.Parse(splitedArgs["rooms-size-y"]);
            _wall_doors_x = int.Parse(splitedArgs["rooms-door-count-x"]);
            _wall_doors_y = int.Parse(splitedArgs["rooms-door-count-y"]);
            _door_open_p = double.Parse(splitedArgs["rooms-door-open-prob"]);
            _barier_p = double.Parse(splitedArgs["rooms-barier-prob"]);
            
            GridSizeX = _num_rooms_x + 1 + (_room_size_x * _num_rooms_x);
            GridSizeY = _num_rooms_y + 1 + (_room_size_y * _num_rooms_y);
        }

        public override bool ValidArgs()
        {
            if (_num_rooms_x == 0 || _num_rooms_y == 0 || _room_size_x == 0 || _room_size_y == 0)
            {
                Console.WriteLine("Some rooms params should not be 0 - quitting");
                return false;
            }
            if (_wall_doors_y > _room_size_y || _wall_doors_x > _room_size_x)
            {
                Console.WriteLine("more doors than wall size - quitting");
                return false;
            }
            return true;
        }

        public override void AddBlockedLocationsStartAndGoal()
        {
            //Draw basic map - only walls
            for (int i = 0; i <= _num_rooms_y; i++)
            {
                DrawWall(i * (_room_size_y + 1), 0, 0, 1);
            }
            for (int i = 0; i <= _num_rooms_x; i++)
            {
                DrawWall( 0, i * (_room_size_x + 1), 1, 0);
            }
            
            //add start and goal locations
            Board[1, 1] = 2;
            Board[GridSizeY - 2, GridSizeX - 2] = 3;
            //TODO: randomize start and goal
            
            //Open vertical walls doors
            for (int i = 0; i < _num_rooms_y; i++)
            {
                for (int j = 1; j < _num_rooms_x; j++)
                {
                    OpenWallDoors(i * (_room_size_y + 1) + 1, j * (_room_size_x + 1), 1, 0);
                }
            }

            //Open horizontal walls doors
            for (int i = 1; i < _num_rooms_y; i++)
            {
                for (int j = 0; j < _num_rooms_x; j++)
                {
                    OpenWallDoors(i * (_room_size_y + 1), j * (_room_size_x + 1) + 1, 0, 1);
                }
            }

            //Adding barriers
            int numOfBarriers = (int)(_barier_p * (_room_size_x * _room_size_y) * (_num_rooms_x * _num_rooms_y));
            while (numOfBarriers > 0)
            {
                int x = _r.Next(GridSizeX - 1) + 1; //no frame
                int y = _r.Next(GridSizeY - 1) + 1; //no frame
                if (Board[y, x] == 0)
                {
                    Board[y, x] = 1;
                    numOfBarriers--;
                }
            }
        }

        private void DrawWall(int startY, int startX, int offY, int offX)
        {
            int x = startX;
            int y = startY;
            while (x < GridSizeX && y < GridSizeY)
            {
                Board[y, x] = 1;
                x += offX;
                y += offY;
            }
        }

        private void OpenWallDoors(int wallStartY, int wallStartX, int offY, int offX)
        {
            int numOfDoors = (offY == 0) ? _wall_doors_x : _wall_doors_y;
            int wallSize = (offY == 0) ? _room_size_x : _room_size_y;
            while (numOfDoors > 0)
            {
                int doorOffset = _r.Next(wallSize);

                if (Board[wallStartY + (doorOffset * offY), wallStartX + (doorOffset * offX)] == 1)
                {
                    numOfDoors--;
                    if (_r.NextDouble() < _door_open_p)
                    {
                        Board[wallStartY + (doorOffset * offY), wallStartX + (doorOffset * offX)] = 0;
                    }
                }
            }
        }

        public override string GetFileName(int id, int setSize)
        {
            string outFileName = "ROOMS-" + id + "-of-" + setSize + "-R[" + _num_rooms_x + 'x' + _num_rooms_y + "]-S[" +
                                 _room_size_x + 'x' + _room_size_y + "]-Wd[" + _wall_doors_x + '_' + _wall_doors_y + "]-Pd[" + _door_open_p + "]-Pb[" + _barier_p + "].grd";
            return outFileName;
        }

    }
}
