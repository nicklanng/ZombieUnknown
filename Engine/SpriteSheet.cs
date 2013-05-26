using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine
{
    public class SpriteSheet
    {
        public string Name { get; private set; }

        public Texture2D Texture { get; private set; }

        public SpriteSheetFrameDefinition Frames { get; private set; }

        public SpriteSheet(string name, Texture2D texture)
        {
            Name = name;
            Texture = texture;

            Frames = new SpriteSheetFrameDefinition();
        }

        public void AddFrame(string name, Rectangle rectangle)
        {
            Frames.Add(name, rectangle);
        }

        public Rectangle GetFrameRectangle(string name)
        {
            return Frames.GetFrame(name);
        }
    }
}
