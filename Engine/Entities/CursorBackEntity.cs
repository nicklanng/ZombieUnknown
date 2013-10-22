using Engine.Maps;
using Engine.Sprites;
using Microsoft.Xna.Framework;

namespace Engine.Entities
{
    public class CursorBackEntity : Entity
    {
        public CursorBackEntity(string name, Sprite sprite)
            : base(name, sprite, new Coordinate())
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
