using Microsoft.Xna.Framework;

namespace Engine.Sprites
{
    public class StaticSprite : Sprite
    {
        public StaticSprite(string name, SpriteSheet spriteSheet, Vector2 center, string frameName)
            : base(name, spriteSheet, center)
        {
            SpriteSheetRectangle = SpriteSheet.GetFrameRectangle(frameName);
        }
    }
}
