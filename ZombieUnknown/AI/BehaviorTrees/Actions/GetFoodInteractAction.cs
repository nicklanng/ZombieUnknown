using Engine.AI.BehaviorTrees.Actions;

namespace ZombieUnknown.AI.BehaviorTrees.Actions
{
    class GetFoodInteractAction : InteractAction
    {
        protected override string InteractionText
        {
            get { return "Get Food"; }
        }
    }
}
