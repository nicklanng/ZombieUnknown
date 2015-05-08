using System.Collections.Generic;
using System.Linq;
using Engine.Entities;

namespace Engine.AI.UtilityBehaviors
{
    public static class UtilityBehaviorRepository
    {
        private static List<IUtilityBehavior> _utilityBehaviors;

        public static void Initialize()
        {
            _utilityBehaviors = new List<IUtilityBehavior>();
        }

        public static void RegisterUtilityBehavior(IUtilityBehavior utilityBehavior)
        {
            _utilityBehaviors.Add(utilityBehavior);
        }

        public static IUtilityBehavior ChooseUtilityBehavior(PhysicalEntity entity)
        {
            var utilityList = _utilityBehaviors.OrderByDescending(x => x.Utility(entity)).ToList();
            var utility = utilityList.First();
            return utility.Utility(entity) < 0.0001 ? null : utility;
        }
    }
}
