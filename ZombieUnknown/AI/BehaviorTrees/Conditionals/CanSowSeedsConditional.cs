using System.Linq;
using Engine;
using Engine.AI.BehaviorTrees;
using Engine.AI.BehaviorTrees.Conditionals;
using Engine.Entities.Interactions;
using Microsoft.Xna.Framework;
using ZombieUnknown.Entities;
using ZombieUnknown.Entities.Interactions;
using ZombieUnknown.Entities.Mobiles;

namespace ZombieUnknown.AI.BehaviorTrees.Conditionals
{
    internal class CanSowSeedsConditional : BehaviorConditional
    {
        protected override bool Test(Blackboard blackboard)
        {
            var actor = (Human)blackboard["subject"];

            var interactionTargetLocation = (Vector2)blackboard["InteractionTargetLocation"];
            var entities = GameState.Map.GetEntitiesAt(interactionTargetLocation);
            var subject = entities.LastOrDefault() as CultivatedLand;
            if (subject == null)
            {
                return false;
            }
            return actor.CanPerformInteractionOn(subject)
                        .OfType<SowSeedsInteraction>();
        }
    }
}