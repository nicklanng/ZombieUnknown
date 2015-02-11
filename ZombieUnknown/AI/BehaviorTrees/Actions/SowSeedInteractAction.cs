using Engine;
using Engine.AI;
using Engine.AI.BehaviorTrees;
using Engine.AI.BehaviorTrees.Actions;
using Engine.Entities.Interactions;
using ZombieUnknown.Entities;
using ZombieUnknown.Entities.Interactions;
using ZombieUnknown.Entities.Mobiles;

namespace ZombieUnknown.AI.BehaviorTrees.Actions
{
    internal class SowSeedInteractAction : InteractAction
    {
        protected override GoalStatus Action(Blackboard blackboard)
        {
            var target = GameState.InteractionObject as CultivatedLand;

            if (target == null)
            {
                return GoalStatus.Failed;
            }

            var actor = (Human)blackboard["subject"];
            blackboard["TargetInteraction"] = InteractionManager.CreateTargetedInteraction<SowSeedsInteraction, CultivatedLand, Human>(target, actor);

            return base.Action(blackboard);
        }
    }
}