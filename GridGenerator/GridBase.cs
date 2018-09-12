
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GridGenerator
{
    public abstract class GridBase
    {
        protected int GridSizeX = 0;
        protected int GridSizeY = 0;
        protected int[,] Board;
        
        public void InitBoard()
        {
            Board = new int[GridSizeY, GridSizeX];
        }
        
        public bool GoalReachable()
        {
            List<int> open = new List<int>();
            HashSet<int> closed = new HashSet<int>();

            //int startPos = 1 * _grid_size_x + 1;//This is (1,1)
            //int goalPos = grid_size_x * grid_size_y - grid_size_x - 2; //GOAL[grid_size_y - 2, grid_size_x - 2];
            int startPos = -1;
            int goalPos = -1;

            for (int i = 0; i < GridSizeY; i++)
            {
                for (int j = 0; j < GridSizeX; j++)
                {
                    if (Board[i, j] == 2)
                    {
                        startPos = i * GridSizeX + j;
                    } else if (Board[i, j] == 3)
                    {
                        goalPos = i * GridSizeX + j;
                    }
                }
            }
            if (startPos == -1 || goalPos == -1)
            {
                throw new NoStartOrGoalException();
            }

            open.Add(startPos);
            closed.Add(startPos);
            while (open.Count != 0)
            {
                int current = open.ElementAt(0);
                open.RemoveAt(0);
                List<int> childs = Expand(current);
                foreach (var child in childs)
                {
                    if (child == goalPos)
                    {
                        return true;
                    }
                    if (Board[child / GridSizeX, child % GridSizeX] != 1)
                    {
                        if (!closed.Contains(child))
                        {
                            closed.Add(child);
                            open.Add(child);
                        }
                    }
                }
            }
            return false;
        }

        private List<int> Expand(int spot)
        {
            List<int> childrens = new List<int>();
            int x = spot % GridSizeX;
            int y = spot / GridSizeY;
            if (x + 1 < GridSizeX)
            {
                childrens.Add(spot + 1);
            }
            if (x - 1 >= 0)
            {
                childrens.Add(spot - 1);
            }
            if (y + 1 < GridSizeY)
            {
                childrens.Add(spot + GridSizeX);
            }
            if (y - 1 >= 0)
            {
                childrens.Add(spot - GridSizeX);
            }
            return childrens;
        }




        public string GetGrid()
        {
            char c;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < GridSizeY; i++)
            {
                for (int j = 0; j < GridSizeX; j++)
                {
                    switch (Board[i, j])
                    {
                        case 0:
                            c = ' ';
                            break;
                        case 1:
                            c = '#';
                            break;
                        case 2:
                            c = '@';
                            break;
                        case 3:
                            c = '*';
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                    sb.Append(c);
                }
                sb.Append(Environment.NewLine);
            }
            return sb.ToString();
        }

        public abstract bool ValidArgs();

        public abstract void AddBlockedLocationsStartAndGoal();

        public abstract string GetFileName(int id, int setSize);
        
        public class NoStartOrGoalException : Exception
        {
        }
    }
}
