using Microsoft.Xna.Framework;

namespace Engine.AI.Steering
{
    public interface ILookAheadBehavior
    {
        Vector2 GetForce(IActor actor, Vector2 plannedVelocity);
    }
}
