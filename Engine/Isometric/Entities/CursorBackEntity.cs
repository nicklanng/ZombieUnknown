using Engine.Sprites;

namespace Engine.Isometric.Entities
{
    internal class CursorBackEntity : Entity
    {
        internal CursorBackEntity(string name, Sprite sprite) 
            : base(name, sprite)
        {
            ZIndex = short.MinValue;
        }
    }g
}
