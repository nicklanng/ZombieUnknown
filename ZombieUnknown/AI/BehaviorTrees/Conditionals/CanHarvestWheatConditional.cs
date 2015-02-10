using System.Linq;
using Engine;
using Engine.AI.BehaviorTrees;
using Engine.AI.BehaviorTrees.Conditionals;
using Microsoft.Xna.Framework;
using ZombieUnknown.Entities.Interactions;
using ZombieUnknown.Entities.Mobiles;

namespace ZombieUnknown.AI.BehaviorTrees.Conditionals
{
    internal class CanHarvestWheatConditional : BehaviorConditional
    {
        protected override bool Test(Blackboard blackboard)
        {
            var actor = (Human)blackboard["subject"];

            var interactionTargetLocation = (Vector2)blackboard["InteractionTargetLocation"];
            var entities = GameState.Map.GetEntitiesAt(interactionTargetLocation);
            var subject = entities.LastOrDefault();
            if (subject == null)
            {
                return false;
            }

            if (subject.Interactions.ContainsKey(HarvestWheatInteraction.Text) == false)
            {
                return false;
            }
            var interactionAction = subject.Interactions[HarvestWheatInteraction.Text];

            return interactionAction.IsPossible(actor);
        }
    }
}