using System;
using Engine.Entities;
using Engine.Maps;
using Engine.Pathfinding;
using Microsoft.Xna.Framework;

namespace Engine.AI.BehaviorTrees.Actions
{
    public class MoveTowardsTarget : BehaviorAction
    {
        protected override GoalStatus Action(Blackboard blackboard)
        {
            var entity = (MobileEntity)blackboard["subject"];

            var target = (Vector2) blackboard["TargetCoordinate"];


            var distanceToTarget = (target - entity.MapPosition);
            var direction = Direction.GetDirectionFromVector(distanceToTarget);
            entity.FaceDirection(direction);
            entity.TransitionState("walk");

            var moveAmount = distanceToTarget;
            moveAmount.Normalize();
            moveAmount = moveAmount * entity.MaxVelocity * 0.001f;

            Vector2 estimatedEndPosition;
            if (moveAmount.Length() >= distanceToTarget.Length())
            {
                estimatedEndPosition = target;
                
                entity.TransitionState("idle");
            }
            else
            {
                estimatedEndPosition = entity.MapPosition + moveAmount;
            }

            var movementblocker = (IMovementBlocker)entity;
            if (movementblocker == null)
            {
                entity.MapPosition = estimatedEndPosition;
                return GoalStatus.Completed;
            }

            var canMoveThere = GameState.Map.IsPositionClear(movementblocker, estimatedEndPosition, 0.5f);
            if (canMoveThere)
            {
                entity.MapPosition = estimatedEndPosition;
                return GoalStatus.Completed;
            }

            return GoalStatus.Failed;
        }

        
    }
}
