using Engine.Entities;
using Engine.Maps;
using Microsoft.Xna.Framework;

namespace Engine.AI
{
    public class TraverseEdgeGoal<T> : Goal where T : MoveableEntity
    {
        private readonly T _entity;
        private Coordinate _origin;
        private Coordinate _target;

        public TraverseEdgeGoal(T entity, Coordinate target)
        {
            _entity = entity;
            _target = target;
        }

        public override void Activate()
        {
            _origin = _entity.Coordinate;

            // set entity state or animation or something
        }

        public override void Process(GameTime gameTime)
        {
            base.Process(gameTime);

            var distanceToTarget = (_target.ToVector2() - _entity.MapPosition);
            var moveAmount = distanceToTarget;
            moveAmount.Normalize();
            moveAmount = moveAmount * (float)gameTime.ElapsedGameTime.TotalSeconds * 0.5f;

            if ((_entity.MapPosition - _origin.ToVector2()).Length() > (_target.ToVector2() - _origin.ToVector2()).Length() / 2)
            {
                // move entity to next board position
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
            // set entity state or animation or something
        }
    }
}
