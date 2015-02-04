using Microsoft.Xna.Framework;
using Engine.Entities;
using Engine.Maps;

namespace Engine.AI
{
    public class TraverseEdgeGoal : Goal
    {
        private readonly PhysicalEntity _entity;

        private Coordinate _origin;
        private readonly Coordinate _target;
        private readonly bool _run;
        private bool _tileSwapped;

        public TraverseEdgeGoal(PhysicalEntity entity, Coordinate target, bool run = false)
        {
            _entity = entity;
            _target = target;
            _run = run;
        }

        public override void Activate()
        {
            base.Activate();

            _origin = _entity.GetCoordinate();

            var isTargetTileBlocked = GameState.Map.GetTile(_target).IsBlocked;
            isTargetTileBlocked = false;
            if (isTargetTileBlocked)
            {
                GoalStatus = GoalStatus.Failed;
            }
            else
            {
                GameState.Map.GetTile(_target).IsBlocked = true;

                var directionVector = _target - _origin;
                var direction = Direction.CoordinateDirectionMap [directionVector];

                _entity.FaceDirection (direction);
                _entity.SetAnimation(_run ? "run" : "walk");
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
    }
}
