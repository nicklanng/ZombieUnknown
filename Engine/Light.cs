using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine
{
    public class Light
    {
        private readonly Sprite _sprite;

        public Tile Parent { get; set; }

        public Color Color { get; private set; }

        public float Range { get; private set; }

        public Light(Sprite lightSprite, Color color, float range)
        {
            _sprite = lightSprite;
            Color = color;
            Range = range;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            _sprite.Draw(spriteBatch, position, Color);
        }
    }
}
