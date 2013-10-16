using Engine.Sprites;

namespace Engine.Entities
{
    public class CursorBackEntity : Entity
    {
        internal CursorBackEntity(string name, Sprite sprite, ISpriteDrawer spriteDrawer)
            : base(name, sprite, spriteDrawer)
        {
            ZIndex = short.MinValue;
        }
    }
}
