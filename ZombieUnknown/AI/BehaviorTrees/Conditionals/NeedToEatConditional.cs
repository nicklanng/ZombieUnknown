﻿using Engine.AI.BehaviorTrees;
using Engine.AI.BehaviorTrees.Conditionals;
using ZombieUnknown.Entities.Mobiles;

namespace ZombieUnknown.AI.BehaviorTrees.Conditionals
{
    class NeedToEatConditional : BehaviorConditional
    {
        protected override bool Test(Blackboard blackboard)
        {
            var human = (Human)blackboard["subject"];

            return human.Hunger < 20;
        }
    }
}
