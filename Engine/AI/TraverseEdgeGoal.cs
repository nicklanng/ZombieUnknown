using Microsoft.Xna.Framework;
using Engine.Entities;
using Engine.Maps;

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

            _origin = _entity.GetCoordinate();

            var isTargetTileBlocked = GameState.Map.GetTile(_target).IsBlocked;
            if (isTargetTileBlocked)
            {
                GoalStatus = GoalStatus.Failed;
            }
            else
            {
                GameState.Map.GetTile(_target).IsBlocked = true;

                var directionVector = _target - _origin;
                var direction = Direction.CoordinateDirectionMap [directionVector];

                _entity.FaceDirection (direction, GameState.GameTime);
            }
        }

        public override void Process()
        {
            base.Process();

            if (!IsActive)
            {
                return;
            }

            var distanceToTarget = ((Vector2)_target - _entity.MapPosition);
            var moveAmount = distanceToTarget;
            moveAmount.Normalize();
            moveAmount = moveAmount * _entity.Speed * 0.001f;

            if (!_tileSwapped && (_entity.MapPosition - (Vector2)_origin).Length() > ((Vector2)_target - (Vector2)_origin).Length() / 2)
            {
                GameState.Map.GetTile(_origin).IsBlocked = false;
                GameState.Map.RemoveEntity(_entity);
                _entity.SetCoordinate(_target);
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
    }
}
