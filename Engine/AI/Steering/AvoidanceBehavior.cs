using Engine.Extensions;
using Engine.Maths;
using Microsoft.Xna.Framework;

namespace Engine.AI.Steering
{
    public class AvoidanceBehavior : ILookAheadBehavior
    {
        private const float MaxAvoidanceForce = 1f;
        private const float MaxSeeAhead = 3;

        public Vector2 GetForce(IActor actor, Vector2 plannedVelocity)
        {
            var ahead = actor.MapPosition + plannedVelocity.NormalizeFixed() * MaxSeeAhead;
            var ahead2 = actor.MapPosition + plannedVelocity.NormalizeFixed() * MaxSeeAhead * 0.5f;
            var ahead3 = actor.MapPosition + plannedVelocity.NormalizeFixed() * MaxSeeAhead * 0.25f;
            var ahead4 = actor.MapPosition + plannedVelocity.NormalizeFixed() * MaxSeeAhead * 0.125f;

            // choose the closest actor that we will bump into
            IActor actorToAvoid = null;
            var distance = float.MaxValue;
            foreach (var otherActor in GameState.Actors)
            {
                if (otherActor != actor)
                {
                    var collision = LineIntersectsCircle(ahead, ahead2, ahead3, ahead4, otherActor);
                    if (!collision) continue;

                    var newDistance = actor.MapPosition.DistanceTo(otherActor.MapPosition);
                    if (newDistance < distance)
                    {
                        distance = newDistance;
                        actorToAvoid = otherActor;
                    }
                }
            }

            // if we found an obstacle, move around him
            if (actorToAvoid != null)
            {
                var lineToObstacle = new Line(actor.MapPosition, actorToAvoid.MapPosition);
                var avoidance = lineToObstacle.Normal;

                return (avoidance * MaxAvoidanceForce).Truncate(1);
            }

            return Vector2.Zero;
        }

        private static bool LineIntersectsCircle(Vector2 ahead, Vector2 ahead2, Vector2 ahead3, Vector2 ahead4, IActor obstacle)
        {
            return ahead.DistanceTo(obstacle.MapPosition) <= obstacle.Radius ||
                   ahead2.DistanceTo(obstacle.MapPosition) <= obstacle.Radius ||
                   ahead3.DistanceTo(obstacle.MapPosition) <= obstacle.Radius ||
                   ahead4.DistanceTo(obstacle.MapPosition) <= obstacle.Radius;
        }
    }
}
