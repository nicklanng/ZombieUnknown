using Engine.Entities;
using Engine.Maps;

namespace Engine.AI
{
    public class TraverseEdgeGoal : Goal
    {
        private readonly MoveableEntity _entity;

        private Coordinate _origin;
        private Coordinate _target;
        private bool _tileSwapped;

        public TraverseEdgeGoal(MoveableEntity entity, Coordinate target)
        {
            _entity = entity;
            _target = target;
        }

        public override void Activate()
        {
            base.Activate();

            _origin = _entity.Coordinate;

            // set entity state or animation or something
        }

        public override void Process()
        {
            base.Process();

            var distanceToTarget = (_target.ToVector2() - _entity.MapPosition);  
            var moveAmount = distanceToTarget;
            moveAmount.Normalize();
            moveAmount = moveAmount * 0.01f;

            if (!_tileSwapped && (_entity.MapPosition - _origin.ToVector2()).Length() > (_target.ToVector2() - _origin.ToVector2()).Length() / 2)
            {
                GameState.Map.RemoveEntity(_entity);
                GameState.Map.AddEntity(_entity, _target);

                _tileSwapped = true;
            }

            if (moveAmount.Length() < distanceToTarget.Length())
            {
                _entity.MapPosition += moveAmount;
            }
            else
            {
                _entity.Coordinate = _target;
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
