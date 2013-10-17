using Engine.Sprites;

namespace Engine.Entities
{
    public abstract class MoveableEntity : Entity
    {
        protected MoveableEntity(string name, Sprite sprite)
            : base(name, sprite)
        {
        }
    }
}
