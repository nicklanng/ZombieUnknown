using System.Collections.Generic;
using System.Linq;
using Engine.Entities;
using Engine.Maps;
using Microsoft.Xna.Framework;

namespace Engine.AI.BehaviorTrees.Basic
{
    public class TakeOneStepAction : BehaviorAction
    {
        public TakeOneStepAction(Blackboard blackboard) 
            : base(blackboard)
        {
        }
        
        protected override GoalStatus Action()
        {
            var entity = (PhysicalEntity) Blackboard["Entity"];
            var currentPath = (List<Coordinate>)Blackboard["MovementPath"];
            var nextStep = currentPath.First();

            var directionVector = GetDirectionVector(entity.MapPosition, nextStep);
            var direction = Direction.CoordinateDirectionMap[directionVector];
            entity.FaceDirection(direction);
            entity.SetAnimation("walk");

            var distanceToTarget = ((Vector2)nextStep - entity.MapPosition);
            var moveAmount = distanceToTarget;
            moveAmount.Normalize();
            moveAmount = moveAmount * entity.Speed * 0.001f;

            if (moveAmount.Length() >= distanceToTarget.Length())
            {
                entity.MapPosition = nextStep;
                Blackboard["MovementPath"] = currentPath.Skip(1);

                return GoalStatus.Completed;
            }

            entity.MapPosition += moveAmount;

            return GoalStatus.Active;
        }

        private static Vector2 GetDirectionVector(Vector2 currentPosition, Vector2 targetPosition)
        {
            var x = 0;
            var y = 0;

            var deltaX = (targetPosition - currentPosition).X;
            if (deltaX > 0)
            {
                x = 1;
            }
            else if (deltaX < 0)
            {
                x = -1;
            }

            var deltaY = (targetPosition - currentPosition).Y;
            if (deltaY > 0)
            {
                y = 1;
            }
            else if (deltaY < 0)
            {
                y = -1;
            }

            return new Vector2(x, y);
        }
    }
}

