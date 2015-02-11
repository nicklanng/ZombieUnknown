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
    internal class SowSeedsInteractAction : InteractAction
    {
        protected override GoalStatus Action(Blackboard blackboard)
        {
            var target = GameState.InteractionObject as CultivatedLand;

            if (target == null)
            {
                return GoalStatus.Failed;
            }

            var actor = (Human)blackboard["subject"];
            blackboard["TargetInteraction"] = InteractionManager.CreateTargetedInteractionFor(target, actor)
                                                                .WithInteraction<SowSeedsInteraction>();

            return base.Action(blackboard);
        }
    }
}