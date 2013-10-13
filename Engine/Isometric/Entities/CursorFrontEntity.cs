using Engine.Sprites;

namespace Engine.Isometric.Entities
{
    internal class CursorFrontEntity : Entity
    {
        internal CursorFrontEntity(string name, Sprite sprite) 
            : base(name, sprite)
        {
            ZIndex = short.MaxValue;
        }
    }
}
