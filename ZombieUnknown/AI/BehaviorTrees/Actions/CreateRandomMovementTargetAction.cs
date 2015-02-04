using Engine;
using Engine.AI;
using Engine.AI.BehaviorTrees;
using Engine.AI.BehaviorTrees.Actions;
using Engine.Maps;

namespace ZombieUnknown.AI.BehaviorTrees.Actions
{
    public class CreateRandomMovementTargetAction : BehaviorAction
    {
        protected override GoalStatus Action(Blackboard blackboard)
        {
            var newLocationX = GameState.RandomNumberGenerator.Next(GameState.Map.Width);
            var newLocationY = GameState.RandomNumberGenerator.Next(GameState.Map.Height);

            blackboard["TargetCoordinate"] = new Coordinate(newLocationX, newLocationY);

            return GoalStatus.Completed;
        }
    }
}
