using Engine.Sprites;
using Microsoft.Xna.Framework;

namespace Engine.Entities
{
    public interface IBaseEntityFactory
    {
        Light CreateLight(string name, StaticSprite sprite, Color color, short range);
        CursorFrontEntity CreateFrontCursor(string name, StaticSprite sprite);
        CursorBackEntity CreateBackCursor(string name, StaticSprite sprite);
    }
}
