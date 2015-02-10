using System.Linq;
using Engine;
using Engine.AI.BehaviorTrees;
using Engine.AI.BehaviorTrees.Conditionals;
using Microsoft.Xna.Framework;
using ZombieUnknown.Entities.Interactions;
using ZombieUnknown.Entities.Mobiles;
using ZombieUnknown.InventoryObjects;

namespace ZombieUnknown.AI.BehaviorTrees.Conditionals
{
    class CanPlantWheatConditional : BehaviorConditional
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

            if (subject.Interactions.ContainsKey(SowSeedInteraction.Text) == false)
            {
                return false;
            }
            var interactionAction = subject.Interactions[SowSeedInteraction.Text];

            return interactionAction.IsPossible(actor);
        }
    }
}