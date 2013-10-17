using Engine.Sprites;

namespace Engine.Entities
{
    public class CursorFrontEntity : Entity
    {
        public CursorFrontEntity(string name, Sprite sprite)
            : base(name, sprite)
        {
            ZIndex = short.MaxValue;
        }
    }
}
