using Engine.Extensions;
using Microsoft.Xna.Framework;

namespace Engine.AI.Steering
{
    public class AvoidActorsBehavior : IBehavior
    {
        private Vector2 _compositeForce;

        public Vector2 GetForce(IActor actor)
        {
            _compositeForce = Vector2.Zero;

            foreach (var otherActor in GameState.Actors)
            {
                if (otherActor == actor)
                {
                    continue;
                }

                var directionOfActor = actor.MapPosition - otherActor.MapPosition;
                var distance = directionOfActor.Length();
                if (distance < actor.Radius)
                {
                    var magnitude = actor.Radius - distance;
                    _compositeForce += directionOfActor*(1 - magnitude*magnitude);
                }
            }

            return _compositeForce.Truncate(1) * 1.1f;
        }
    }
}
