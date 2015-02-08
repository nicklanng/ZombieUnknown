using System.Linq;
using Engine;
using Engine.AI;
using Engine.AI.BehaviorTrees;
using Engine.AI.BehaviorTrees.Actions;
using Engine.Maps;

namespace ZombieUnknown.AI.BehaviorTrees.Actions
{
    public class GetFoodSourceAction : BehaviorAction
    {
        protected override GoalStatus Action(Blackboard blackboard)
        {
            blackboard["InteractionTargetLocation"] = GameState.InteractionObject.MapPosition;
            blackboard["TargetCoordinate"] = (Coordinate)(GameState.InteractionObject.MapPosition + GameState.InteractionObject.AccessPositions.First().PositionOffset);

            return GoalStatus.Completed;
        }
    }
}

