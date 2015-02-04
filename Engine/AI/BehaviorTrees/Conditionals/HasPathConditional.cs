using System.Collections.Generic;
using Engine.Maps;

namespace Engine.AI.BehaviorTrees.Conditionals
{
    public class HasPathConditional : BehaviorConditional
    {
        protected override bool Test(Blackboard blackboard)
        {
            var currentPath = (List<Coordinate>) blackboard["MovementPath"];
            return currentPath.Count > 0;
        }
    }
}
