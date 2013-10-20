using Engine.Sprites;
using Microsoft.Xna.Framework;

namespace Engine.Entities
{
    public class CursorFrontEntity : Entity
    {
        public CursorFrontEntity(string name, Sprite sprite)
            : base(name, sprite, new Vector2())
        {
            ZIndex = short.MaxValue;
        }

        public override void Draw(Color light)
        {
            light = Color.White;
            base.Draw(light);
        }
    }
}
