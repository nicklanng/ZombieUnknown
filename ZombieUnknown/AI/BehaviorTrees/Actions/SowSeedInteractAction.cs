using Engine.AI.BehaviorTrees.Actions;
using ZombieUnknown.Entities.Interactions;

namespace ZombieUnknown.AI.BehaviorTrees.Actions
{
    class SowSeedInteractAction : InteractAction
    {
        protected override string InteractionText
        {
            get { return SowSeedInteraction.Text; }
        }
    }
}
