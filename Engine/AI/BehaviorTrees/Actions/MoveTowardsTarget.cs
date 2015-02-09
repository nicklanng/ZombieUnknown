using System;
using Engine.Entities;
using Engine.Maps;
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
            moveAmount = moveAmount * entity.Speed * 0.001f;

            if (moveAmount.Length() >= distanceToTarget.Length())
            {
                entity.MapPosition = target;
                
                entity.TransitionState("idle");
            }
            else
            {
                entity.MapPosition += moveAmount;
            }


            return GoalStatus.Completed;
        }

        
    }
}
