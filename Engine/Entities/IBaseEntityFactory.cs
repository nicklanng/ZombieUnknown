using Engine.Sprites;
using Microsoft.Xna.Framework;

namespace Engine.Entities
{
    public interface IBaseEntityFactory
    {
        Light CreateLight(string name, StaticSprite sprite, Color color, short range);
    }
}
