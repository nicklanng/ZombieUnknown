using System.Collections.Generic;
using System.Linq;
using Engine.Extensions;
using Microsoft.Xna.Framework;

namespace Engine.AI.Steering
{
    public class FollowPathBehavior : IBehavior
    {
        private readonly List<Vector2> _waypoints;

        public FollowPathBehavior(List<Vector2> waypoints)
        {
            _waypoints = waypoints;
        }

        public Vector2 GetForce(IActor actor)
        {
            // if no waypoints do nothing
            if (!_waypoints.Any())
            {
                actor.FollowPathBehavior = null;
                return Vector2.Zero;
            }

            // try to skip uneccesary waypoints
            var currentWaypoint = _waypoints.First();
            while (_waypoints.Count > 1 && GameState.PathfindingMap.DirectPathBetween(actor.MapPosition, _waypoints.ElementAt(1)))
            {
                _waypoints.RemoveAt(0);
                currentWaypoint = _waypoints.First();
            }

            // if next waypoint is not in direct line, we need to abandon and restart
            if (!GameState.PathfindingMap.DirectPathBetween(actor.MapPosition, currentWaypoint))
            {
                actor.FollowPathBehavior = null;
                return Vector2.Zero;
            }

            // arrival
            if (actor.MapPosition.DistanceTo(currentWaypoint) < 0.09f)
            {
                _waypoints.RemoveAt(0);
                if (!_waypoints.Any())
                {
                    actor.FollowPathBehavior = null;
                    return Vector2.Zero;
                }
                currentWaypoint = _waypoints.First();
            }

            // move towards
            var desiredMovement = (currentWaypoint - actor.MapPosition).NormalizeFixed();
            return desiredMovement;
        }
    }
}
