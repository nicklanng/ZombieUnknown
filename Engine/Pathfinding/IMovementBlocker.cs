using Microsoft.Xna.Framework;

namespace Engine.Pathfinding
{
    public interface IMovementBlocker
    {
        bool BlocksTile { get; }
        bool BlocksDiagonals { get; }

        Vector2 MapPosition { get; }
    }
}
