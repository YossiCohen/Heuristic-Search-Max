
using MaxSearchAlg;

namespace Grid.Domain
{
    public class GoalOnLocation: IGoalCheckMethod
    {
        private Location _validGoalLocation;

        public GoalOnLocation(Location goalLocation)
        {
            _validGoalLocation = goalLocation;
        }
        public bool ValidGoal(INode node)
        {
            var gridNode = node as GridSearchNode;
            return gridNode != null && (gridNode.HeadLocation.Equals(_validGoalLocation));
        }
    }
}