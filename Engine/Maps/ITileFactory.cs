using Engine.Sprites;
using Microsoft.Xna.Framework;

namespace Engine.Maps
{
    public interface ITileFactory
    {
        Tile CreateTile(Vector2 position, Sprite floorSprite, Sprite leftWall, Sprite rightWall, Sprite wallJoinSprite);
    }
}
