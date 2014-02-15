using Microsoft.Xna.Framework;

namespace Engine.Sprites
{
    public class StaticSprite : Sprite
    {
        public StaticSprite(string name, SpriteSheet spriteSheet, Vector2 center, BoundingBox localBoundingBox, string frameName)
            : base(name, spriteSheet, center, localBoundingBox)
        {
            SpriteSheetRectangle = SpriteSheet.GetFrameRectangle(frameName);
        }
    }
}
