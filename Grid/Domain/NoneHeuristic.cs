using System;

namespace Grid.Domain
{
    public class NoneHeuristic : IGridHeuristic
    {
        public int Calc_H(World w, GridSearchNode gridNode)
        {
            return w.Width * w.Height;
        }

        public int Calc_Life_H(World w, GridSearchNode gridNode)
        {
            return (w.Height*(w.Height+1)/2)*w.Width;
        }

        public string GetName()
        {
            return "A None_H";
        }
    }
}