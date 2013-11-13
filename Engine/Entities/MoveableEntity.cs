using Engine.Maps;
using Engine.Sprites;
using Microsoft.Xna.Framework;

namespace Engine.Entities
{
    public abstract class MoveableEntity : Entity
    {
        public Vector2 MapPosition { get; set; }

        protected MoveableEntity(string name, Sprite sprite, Coordinate coordinate)
            : base(name, sprite, coordinate)
        {
        }
    }
}
