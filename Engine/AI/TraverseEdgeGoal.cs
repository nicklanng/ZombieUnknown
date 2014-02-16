using Engine.Entities;
using Engine.Maps;
using Microsoft.Xna.Framework;

namespace Engine.AI
{
    public class TraverseEdgeGoal : Goal
    {
        private readonly DrawableEntity _entity;

        private Coordinate _origin;
        private readonly Coordinate _target;
        private bool _tileSwapped;

        public TraverseEdgeGoal(DrawableEntity entity, Coordinate target)
        {
            _entity = entity;
            _target = target;
        }

        public override void Activate()
        {
            base.Activate();

            _origin = _entity.Coordinate;

            var isTargetTileBlocked = GameState.Map.GetTile(_target).IsBlocked;
            if (isTargetTileBlocked)
            {
                GoalStatus = GoalStatus.Failed;
            }
            else
            {
                GameState.Map.GetTile(_target).IsBlocked = true;
                SetAnimation();
            }
        }

        public override void Process()
        {
            base.Process();

            if (!IsActive)
            {
                return;
            }

            var distanceToTarget = (_target.ToVector2() - _entity.MapPosition);  
            var moveAmount = distanceToTarget;
            moveAmount.Normalize();
            moveAmount = moveAmount * 0.01f;

            if (!_tileSwapped && (_entity.MapPosition - _origin.ToVector2()).Length() > (_target.ToVector2() - _origin.ToVector2()).Length() / 2)
            {
                GameState.Map.GetTile(_origin).IsBlocked = false;
                GameState.Map.RemoveEntity(_entity);
                _entity.Coordinate = _target;
                GameState.Map.AddEntity(_entity);

                _tileSwapped = true;
            }

            if (moveAmount.Length() < distanceToTarget.Length())
            {
                _entity.MapPosition += moveAmount;
            }
            else
            {
                GoalStatus = GoalStatus.Completed;
            }
        }

        public override void Terminate()
        {
            base.Terminate();
            // set entity state or animation or something
        }
        
        private void SetAnimation()
        {
            var animationName = "default";
            var directionVector = _target - _entity.Coordinate;
            if (directionVector == Coordinate.Up)
            {
                animationName = "standingUp";
            }
            if (directionVector == Coordinate.UpLeft)
            {
                animationName = "standingUpLeft";
            }
            if (directionVector == Coordinate.Left)
            {
                animationName = "standingLeft";
            }
            if (directionVector == Coordinate.DownLeft)
            {
                animationName = "standingDownLeft";
            }
            if (directionVector == Coordinate.Down)
            {
                animationName = "standingDown";
            }
            if (directionVector == Coordinate.DownRight)
            {
                animationName = "standingDownRight";
            }
            if (directionVector == Coordinate.Right)
            {
                animationName = "standingRight";
            }
            if (directionVector == Coordinate.UpRight)
            {
                animationName = "standingUpRight";
            }
            _entity.SetAnimation(animationName, GameState.GameTime);
        }
    }
}
