using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Engine.Sprites
{
    public class SpriteSheetFrameDictionary : IEnumerable<KeyValuePair<string, Rectangle>>
    {
        private readonly Dictionary<string, Rectangle> _frames;

        public SpriteSheetFrameDictionary()
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

        public IEnumerator<KeyValuePair<string, Rectangle>> GetEnumerator()
        {
            return _frames.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
