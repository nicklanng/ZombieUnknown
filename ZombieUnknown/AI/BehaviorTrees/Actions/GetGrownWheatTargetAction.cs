using System.Linq;
using Engine;
using Engine.AI;
using Engine.AI.BehaviorTrees;
using Engine.AI.BehaviorTrees.Actions;
using Engine.Entities.Interactions;
using Engine.Maps;
using ZombieUnknown.Entities;
using ZombieUnknown.Entities.Interactions;
using ZombieUnknown.Entities.Mobiles;

namespace ZombieUnknown.AI.BehaviorTrees.Actions
{
    internal class GetGrownWheatTargetAction : BehaviorAction
    {
        protected override GoalStatus Action(Blackboard blackboard)
        {
            var subject = GameState.InteractionObject as Wheat;

            if (subject == null)
            {
                return GoalStatus.Failed;
            }

            blackboard["InteractionTargetLocation"] = GameState.InteractionObject.MapPosition;
            blackboard["TargetCoordinate"] = (Coordinate)(GameState.InteractionObject.MapPosition + GameState.InteractionObject.AccessPositions.First().PositionOffset);
            var actor = (Human)blackboard["subject"];
            blackboard["TargetInteraction"] = TargetInteraction<Wheat, Human, HarvestWheatInteraction>.Create(subject, actor);

            return GoalStatus.Completed;
        }
    }
}