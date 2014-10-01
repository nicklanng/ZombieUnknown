using System.Collections.Generic;
using Engine.Maps;
using Engine.Sprites;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Drawing
{
    public class Console : IUIProvider
    {
        private SpriteBatch _spriteBatch;
        private Font _font;
        private static Queue<string> _lines;

        private Console() { }
        private static readonly Console Instance = new Console();

        public static void Initialize(SpriteBatch spriteBatch, Font font, int numberOfLines)
        {
            Instance._spriteBatch = spriteBatch;
            Instance._font = font;
            _lines = new Queue<string>(numberOfLines);
        }

        public static void WriteLine(string input)
        {
            if (_lines.Count == 10)
            {
                _lines.Dequeue();
            }

            _lines.Enqueue(input);
        }

        public IEnumerable<UIRequest> GetDrawings()
        {
            var maxWidth = _spriteBatch.GraphicsDevice.Viewport.Width;

            var x = 5;
            var y = 5;

            foreach (var line in _lines)
            {
                foreach (var character in line)
                {
                    yield return new UIRequest(_font.GetSprite(character), new Coordinate(x, y), 1000);
                    x = x + _font.Width + 2;

                    if (x > maxWidth)
                    {
                        break;
                    }
                }
                x = 5;
                y = y + _font.Height + 2;
            }
        }

        public static IUIProvider DrawingProvider
        {
            get { return Instance; }
        }
    }
}
