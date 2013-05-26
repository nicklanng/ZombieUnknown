using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Engine
{
    public class SpriteSheetFrameDefinition
    {
        private readonly Dictionary<string, Rectangle> _frames;

        public SpriteSheetFrameDefinition()
        {
            _frames = new Dictionary<string, Rectangle>();
        }

        public void Add(string name, Rectangle rectangle)
        {
            _frames.Add(name, rectangle);
        }

        public Rectangle GetFrame(string name)
        {
            return _frames[name];
        }
    }
}
