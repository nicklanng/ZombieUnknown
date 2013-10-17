using Engine.Sprites;

namespace Engine.Entities
{
    public class CursorBackEntity : Entity
    {
        public CursorBackEntity(string name, Sprite sprite)
            : base(name, sprite)
        {
            ZIndex = short.MinValue;
        }
    }
}
