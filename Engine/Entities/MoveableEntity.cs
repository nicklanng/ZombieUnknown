using Engine.Sprites;
using Microsoft.Xna.Framework;

namespace Engine.Entities
{
    public abstract class MoveableEntity : Entity
    {
        protected MoveableEntity(string name, Sprite sprite, Vector2 mapPosition)
            : base(name, sprite, mapPosition)
        {
        }
    }
}
