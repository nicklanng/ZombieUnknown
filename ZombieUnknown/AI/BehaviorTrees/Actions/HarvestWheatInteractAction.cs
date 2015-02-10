using Engine.AI.BehaviorTrees.Actions;
using ZombieUnknown.Entities.Interactions;

namespace ZombieUnknown.AI.BehaviorTrees.Actions
{
    class HarvestWheatInteractAction : InteractAction
    {
        protected override string InteractionText
        {
            get { return HarvestWheatInteraction.Text; }
        }
    }
}