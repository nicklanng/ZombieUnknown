using Engine;
using Engine.AI;
using Engine.AI.BehaviorTrees;
using Engine.AI.BehaviorTrees.Actions;

namespace ZombieUnknown.AI.BehaviorTrees.Actions
{
    class GetHumanAction : BehaviorAction
    {
        protected override GoalStatus Action(Blackboard blackboard)
        {
            blackboard["TargetCoordinate"] = GameState.ZombieTarget.MapPosition;

            return GoalStatus.Completed;
        }
    }
}
