using Engine.AI.BehaviorTrees;
using Engine.AI.BehaviorTrees.Conditionals;
using ZombieUnknown.Entities.Mobiles;
using ZombieUnknown.InventoryObjects;

namespace ZombieUnknown.AI.BehaviorTrees.Conditionals
{
    class CanPlantWheatConditional : BehaviorConditional
    {
        protected override bool Test(Blackboard blackboard)
        {
            var human = (Human)blackboard["subject"];

            return human.Rig.GetInventories().HasItemOfType<WheatSeedObject>();
        }
    }
}