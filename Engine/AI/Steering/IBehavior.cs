using Microsoft.Xna.Framework;

namespace Engine.AI.Steering
{
    public interface IBehavior
    {
        Vector2 GetForce(IActor actor);
    }
}
