using Engine.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Entities
{
    public class Light : Entity
    {
        public Color Color { get; private set; }

        public float Range { get; private set; }

        public Light(string name, Sprite lightSprite, Color color, float range)
            : base(name, lightSprite)
        {
            Color = color;
            Range = range;
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 position, Color light)
        {
            Sprite.Draw(spriteBatch, position, Color);
        }
    }
}
