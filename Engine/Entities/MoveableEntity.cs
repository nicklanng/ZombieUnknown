using Engine.Maps;
using Engine.Sprites;

namespace Engine.Entities
{
    public abstract class MoveableEntity : Entity
    {
        protected MoveableEntity(string name, Sprite sprite, Coordinate coordinate)
            : base(name, sprite, coordinate)
        {
        }
    }
}
