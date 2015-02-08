using Engine.AI.BehaviorTrees;
using Engine.AI.BehaviorTrees.Conditionals;
using ZombieUnknown.Entities;
using ZombieUnknown.Entities.Mobiles;

namespace ZombieUnknown.AI.BehaviorTrees.Conditionals
{
    class NeedFoodConditional : BehaviorConditional
    {
        protected override bool Test(Blackboard blackboard)
        {
            var human = (Human)blackboard["subject"];
            if (human == null)
            {
                return false;
            }

            return human.Hunger < 20;
        }
    }
}
