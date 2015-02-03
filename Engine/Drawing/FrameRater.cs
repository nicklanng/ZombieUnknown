using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Engine.Maps;
using Engine.Sprites;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Drawing
{
    public class FrameRater : IUIProvider
    {
        private SpriteBatch _spriteBatch;
        private Font _font;

        private int _framerate = 10;
        private int _airate = 10;
        private double _lastFrameUpdate;
        private double _lastAiUpdate;

        private static readonly FrameRater Instance = new FrameRater();

        public static void Initialize(SpriteBatch spriteBatch, Font font)
        {
            Instance._spriteBatch = spriteBatch;
            Instance._font = font;
        }

        public IEnumerable<UIRequest> GetDrawings()
        {
            var maxWidth = _spriteBatch.GraphicsDevice.Viewport.Width;

            var x = maxWidth - 5;
            var y = 5;

            var frameRateString = ("FPS:" + _framerate.ToString(CultureInfo.InvariantCulture)).Reverse();

            foreach (var character in frameRateString)
            {
                x = x - (_font.Width + 2);
                yield return new UIRequest(_font.GetSprite(character), new Coordinate(x, y), 1.0f);
            }

            x = maxWidth - 5;
            y = 5 + _font.Height + 2;

            var aiRateString = ("AI:" + _airate.ToString(CultureInfo.InvariantCulture)).Reverse();

            foreach (var character in aiRateString)
            {
                x = x - (_font.Width + 2);
                yield return new UIRequest(_font.GetSprite(character), new Coordinate(x, y), 1.0f);
            }
        }

        public static void NewFrame(double totalMilliseconds)
        {
            Instance._lastFrameUpdate = totalMilliseconds;
        }

        public static void EndFrame(double totalMilliseconds)
        {
            var timeTaken = totalMilliseconds - Instance._lastFrameUpdate;

            Instance._framerate = (int)(1000 / timeTaken);

            if (Instance._framerate > 60 || Instance._framerate < 0)
            {
                Instance._framerate = 60;
            }
        }

        public static IUIProvider DrawingProvider
        {
            get { return Instance; }
        }

        public static void NewUpdate(double totalMilliseconds)
        {
            Instance._lastAiUpdate = totalMilliseconds;
        }

        public static void EndUpdate(double totalMilliseconds)
        {
            var timeTaken = totalMilliseconds - Instance._lastAiUpdate;

            Instance._airate = (int) (1000 / timeTaken);
            System.Console.WriteLine(timeTaken);

            if (Instance._airate > 60 || Instance._airate < 0)
            {
                Instance._airate = 60;
            }
        }
    }
}
