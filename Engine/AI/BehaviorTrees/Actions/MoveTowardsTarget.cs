using System.Collections.Generic;
using Engine.AI.Steering;
using Engine.Entities;
using Engine.Extensions;
using Microsoft.Xna.Framework;

namespace Engine.AI.BehaviorTrees.Actions
{
    public class MoveTowardsTarget : BehaviorAction
    {
        protected override GoalStatus Action(Blackboard blackboard)
        {
            var entity = (MobileEntity)blackboard["subject"];
            var targetEntity = (ITarget)blackboard["targetEntity"];

            var path = blackboard.GetValue<List<Vector2>>("movementPath");

            if (SavedResult == GoalStatus.Inactive)
            {
                entity.SeekBehavior = null;
                entity.FollowPathBehavior = new FollowPathBehavior(path);
                entity.TransitionState("walk");

                return GoalStatus.Running;
            }

            if (SavedResult == GoalStatus.Running && entity.SeekBehavior == null)
            {
                var directLine = GameState.PathfindingMap.DirectPathBetween(entity.MapPosition, targetEntity.MapPosition);
                if (directLine)
                {
                    entity.FollowPathBehavior = null;
                    entity.SeekBehavior = new SeekBehavior(targetEntity);
                }
            }

            if (entity.MapPosition.DistanceTo(targetEntity.MapPosition) < entity.Radius)
            {
                entity.TransitionState("idle");
                return GoalStatus.Completed;
            }


            return GoalStatus.Running;
        }

        
    }
}
