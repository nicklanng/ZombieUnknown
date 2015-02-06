using System.Collections.Generic;
using System.Globalization;
using Microsoft.Xna.Framework;

namespace Engine.Sprites
{
    public class Font
    {
        private Dictionary<string, Sprite> _characters;

        public Font(SpriteSheet spriteSheet)
        {
            Height = spriteSheet.Frames.GetFrame("a").Height;
            Width = spriteSheet.Frames.GetFrame("a").Width;

            BuildCharacterSprites(spriteSheet);
        }

        public int Width { get; private set; }
        public int Height { get; private set; }

        private void BuildCharacterSprites(SpriteSheet spriteSheet)
        {
            _characters = new Dictionary<string, Sprite>();

            foreach (var frame in spriteSheet.Frames)
            {
                _characters.Add(frame.Key, new StaticSprite(frame.Key, spriteSheet, Vector2.Zero, frame.Key));
            }
        }

        public Sprite GetSprite(char character)
        {
            var keyAsString = character.ToString(CultureInfo.InvariantCulture).ToLower();
            if (_characters.ContainsKey(keyAsString))
            {
                return _characters[keyAsString];
            }

            return _characters["_"];
        }
    }
}
