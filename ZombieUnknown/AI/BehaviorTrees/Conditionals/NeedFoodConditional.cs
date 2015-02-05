using Engine.AI.BehaviorTrees;
using Engine.AI.BehaviorTrees.Conditionals;
using ZombieUnknown.Entities;

namespace ZombieUnknown.AI.BehaviorTrees.Conditionals
{
    class NeedFoodConditional : BehaviorConditional
    {
        protected override bool Test(Blackboard blackboard)
        {
            var human = (Human)blackboard["Entity"];
            if (human == null)
            {
                return false;
            }

            return human.Hunger < 20;
        }
    }
}
