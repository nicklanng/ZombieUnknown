using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Sprites
{
    public abstract class Sprite
    {
        protected SpriteSheet SpriteSheet;
        protected Rectangle SpriteSheetRectangle;

        protected Vector2 Center;

        public string Name { get; private set; }

        public short Width
        {
            get { return (short)SpriteSheetRectangle.Width; }
        }

        public short Height
        {
            get { return (short)SpriteSheetRectangle.Height; }
        }

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

        public virtual void Update(GameTime gameTime)
        {
            
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            Draw(spriteBatch, position, Color.White);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, Color lightColor)
        {
            spriteBatch.Draw(
                SpriteSheet.Texture,
                new Rectangle((int)position.X * EngineSettings.ZoomFactor, (int)position.Y * EngineSettings.ZoomFactor, SpriteSheetRectangle.Width * EngineSettings.ZoomFactor, SpriteSheetRectangle.Height * EngineSettings.ZoomFactor),
                SpriteSheetRectangle,
                lightColor,
                0.0f,
                Center,
                SpriteEffects.None,
                1.0f);
        }
    }
}