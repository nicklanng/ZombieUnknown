using Engine.Sprites;

namespace Engine.Entities
{
    public class CursorFrontEntity : Entity
    {
        internal CursorFrontEntity(string name, Sprite sprite, ISpriteDrawer spriteDrawer)
            : base(name, sprite, spriteDrawer)
        {
            ZIndex = short.MaxValue;
        }
    }
}
