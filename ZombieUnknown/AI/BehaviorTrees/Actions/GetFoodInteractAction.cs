using Engine.AI.BehaviorTrees.Actions;
using ZombieUnknown.Entities.Interactions;

namespace ZombieUnknown.AI.BehaviorTrees.Actions
{
    class GetFoodInteractAction : InteractAction
    {
        protected override string InteractionText
        {
            get { return GetFoodInteraction.Text; }
        }
    }
}
