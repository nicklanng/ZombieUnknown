using Engine.Extensions;
using Engine.Maps;
using Microsoft.Xna.Framework;

namespace Engine.AI.Steering
{
    public class SeekBehavior : IBehavior
    {
        private readonly ITarget _target;

        public SeekBehavior(ITarget target)
        {
            _target = target;
        }

        public Vector2 GetForce(IActor actor)
        {
            var desiredMovement = (_target.MapPosition - actor.MapPosition);
            var distanceToTarget = desiredMovement.Length();
            if (distanceToTarget < actor.Radius)
            {
                return Vector2.Zero;
            }

            var desiredVelocity = desiredMovement.Truncate(actor.MaxVelocity);
            var steering = (desiredVelocity - actor.Velocity).Truncate(actor.MaxVelocity / 1);

            
            return steering;
        }
    }
}
