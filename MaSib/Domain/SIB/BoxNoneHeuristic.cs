
namespace MaSib.Domain.SIB
{
    public class BoxNoneHeuristic : IBoxHeuristic
    {
        public int calc_h(Box s)
        {
            return s.world.MaxPlacesInDimention;
        }
    }
}
