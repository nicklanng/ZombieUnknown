using System.Linq;
using Engine.AI.BehaviorTrees;
using Engine.AI.BehaviorTrees.Conditionals;
using ZombieUnknown.Entities.Mobiles;
using ZombieUnknown.InventoryObjects;

namespace ZombieUnknown.AI.BehaviorTrees.Conditionals
{
    class HasFoodConditional : BehaviorConditional
    {
        protected override bool Test(Blackboard blackboard)
        {
            var human = (Human)blackboard["subject"];
            var inventories = human.Rig.GetInventories();

            foreach (var inventory in inventories)
            {
                var items = inventory.ListItems();
                var hasFood = items.Any(x => x.Item2 is FoodObject);

                if (hasFood) return true;
            }

            return false;
        }
    }
}
