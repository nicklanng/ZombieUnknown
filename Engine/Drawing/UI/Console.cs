using System.Collections.Generic;
using Engine.Maps;
using Engine.Sprites;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Drawing.UI
{
    public class Console : IUIProvider
    {
        private SpriteBatch _spriteBatch;
        private Font _font;
        private static Queue<string> _lines;

        private Console() { }
        private static readonly Console Instance = new Console();
        private static int _numberOfLines;

        public static void Initialize(SpriteBatch spriteBatch, Font font, int numberOfLines)
        {
            _numberOfLines = numberOfLines;
            Instance._spriteBatch = spriteBatch;
            Instance._font = font;
            _lines = new Queue<string>(numberOfLines);
        }

        public static void WriteLine(string input)
        {
            if (_lines.Count == _numberOfLines)
            {
                _lines.Dequeue();
            }

            _lines.Enqueue(input);
        }

        public IEnumerable<UIRequest> GetDrawings()
        {
            var maxWidth = GameState.GraphicsDevice.Viewport.Width;

            var x = 5;
            var y = 5;

            foreach (var line in _lines)
            {
                foreach (var character in line)
                {
                    yield return new UIRequest(_font.GetSprite(character), new Coordinate(x, y), 1.0f);
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
