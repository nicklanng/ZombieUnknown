using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine
{
    public abstract class Sprite
    {
        protected SpriteSheet SpriteSheet;

        protected Vector2 Center;

        public string Name { get; private set; }

        public abstract short Width { get; }

        public abstract short Height { get; }

        protected Sprite(string name, SpriteSheet spriteSheet)
            : this(name, spriteSheet, Vector2.Zero)
        {
        }

        protected Sprite(string name, SpriteSheet spriteSheet, Vector2 center)
        {
            Name = name;
            SpriteSheet = spriteSheet;
            Center = center;
        }

        public abstract void Update(GameTime gameTime);

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            Draw(spriteBatch, position, Color.White);
        }

        public abstract void Draw(SpriteBatch spriteBatch, Vector2 position, Color lightColor);
    }
}