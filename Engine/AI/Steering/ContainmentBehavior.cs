using System;
using Engine.Extensions;
using Engine.Maths;
using Microsoft.Xna.Framework;

namespace Engine.AI.Steering
{
    public class ContainmentBehavior : ILookAheadBehavior
    {
        public Vector2 GetForce(IActor actor, Vector2 plannedVelocity)
        {
            var navMesh = GameState.PathfindingMap.NavMesh;

            // think this hack only work on top and right walls
            var plannedMovement = new Line(actor.MapPosition + new Vector2(0.001f, 0.001f), actor.MapPosition + plannedVelocity);

            var forceVector = Vector2.Zero;

            foreach (var area in navMesh)
            {
                for (var i = 0; i < area.Count; i++)
                {
                    var j = (i + 1)%area.Count;
                    var wall = new Line(new Vector2(area[i].X / 10.0f, area[i].Y / 10.0f), new Vector2(area[j].X / 10.0f, area[j].Y / 10.0f));
                    float howFarThrough;
                    if (plannedMovement.IntersectsLine(wall, out howFarThrough))
                    {
                        var angle = Math.Acos(Vector2.Dot(plannedVelocity.NormalizeFixed(), wall.Normal.NormalizeFixed()));
                        if (angle < MathHelper.PiOver2)
                        {
                            continue;
                        }

                        forceVector += wall.Normal * (1 - howFarThrough) * 1.2f;
                    }
                }
            }

            return forceVector;
        }
    }
}
