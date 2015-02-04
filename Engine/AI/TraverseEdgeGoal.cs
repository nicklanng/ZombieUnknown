using Microsoft.Xna.Framework;
using Engine.Entities;
using Engine.Maps;

namespace Engine.AI
{
    public class TraverseEdgeGoal : Goal
    {
        private readonly PhysicalEntity _entity;

        private readonly Coordinate _target;

        public TraverseEdgeGoal(PhysicalEntity entity, Coordinate target, bool run = false)
        {
            _entity = entity;
            _target = target;
        }
        
        public override void Process()
        {
            base.Process();

            var nextStep = _target;
            if (_entity.MapPosition == (Vector2)nextStep)
            {
                GoalStatus = GoalStatus.Completed;
                return;
            }

            var directionVector = GetDirectionVector(_entity.MapPosition, nextStep);
            var direction = Direction.CoordinateDirectionMap[directionVector];
            _entity.FaceDirection(direction);
            _entity.SetAnimation("walk");

            var distanceToTarget = ((Vector2)nextStep - _entity.MapPosition);
            var moveAmount = distanceToTarget;
            moveAmount.Normalize();
            moveAmount = moveAmount * _entity.Speed * 0.001f;

            if (moveAmount.Length() >= distanceToTarget.Length())
            {
                _entity.MapPosition = nextStep;

                GoalStatus = GoalStatus.Completed;
                return;
            }

            _entity.MapPosition += moveAmount;
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
