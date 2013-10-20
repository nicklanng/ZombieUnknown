using Engine.Sprites;
using Microsoft.Xna.Framework;

namespace Engine.Entities
{
    public class CursorBackEntity : Entity
    {
        public CursorBackEntity(string name, Sprite sprite)
            : base(name, sprite, new Vector2())
        {
            ZIndex = short.MinValue;
        }

        public override void Draw(Color light)
        {
            light = Color.White;
            base.Draw(light);
        }
    }
}
