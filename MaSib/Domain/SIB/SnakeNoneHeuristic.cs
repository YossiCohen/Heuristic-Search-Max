
namespace MaSib.Domain.SIB
{
    public class SnakeNoneHeuristic : ISnakeHeuristic
    {
        public int calc_h(Snake s)
        {
            return s.world.MaxPlacesInDimention;
        }
    }
}
