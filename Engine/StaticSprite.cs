using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine
{
    public class StaticSprite : Sprite
    {
        private readonly string _frameName;
        private Rectangle _spriteSheetRectangle;

        public override short Width
        {
            get { return (short) _spriteSheetRectangle.Width; }
        }

        public override short Height
        {
            get { return (short) _spriteSheetRectangle.Height;  }
        }

        public StaticSprite(string name, SpriteSheet spriteSheet, string frameName, Vector2 center) 
            : base(name, spriteSheet, center)
        {
            _frameName = frameName;
            _spriteSheetRectangle = SpriteSheet.GetFrameRectangle(_frameName);
        }

        public override void Update(GameTime gameTime) { }

        public override void Draw(SpriteBatch spriteBatch, Vector2 position, Color lightColor)
        {
            spriteBatch.Draw(
                SpriteSheet.Texture,
                new Rectangle((int)position.X * Constants.ZoomFactor, (int)position.Y * Constants.ZoomFactor, _spriteSheetRectangle.Width * Constants.ZoomFactor, _spriteSheetRectangle.Height * Constants.ZoomFactor),
                _spriteSheetRectangle,
                lightColor,
                0.0f, 
                Center,
                SpriteEffects.None,
                1.0f);
        }
    }
}
