using Engine.Extensions;
using Microsoft.Xna.Framework;

namespace Engine.AI.Steering
{
    public class QueueBehavior : ILookAheadBehavior
    {
        public Vector2 GetForce(IActor actor, Vector2 plannedVelocity)
        {
            var endPosition = actor.MapPosition + plannedVelocity.Truncate(1) * actor.MaxVelocity * 2;

            foreach (var otherActor in GameState.Actors)
            {
                if (otherActor == actor)
                {
                    continue;
                }
                var distance = endPosition.DistanceTo(otherActor.MapPosition);
                if (distance < actor.Radius)
                {
                    return plannedVelocity * -0.2f;
                }
            }

            return Vector2.Zero;
        }
    }
}
