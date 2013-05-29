using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Engine.Input
{
    public static class Mouse
    {
        private static bool _lmbDown;

        public static event EventHandler MouseMoved;
        public static event EventHandler LmbDown;
        public static event EventHandler LmbUp;

        public static Vector2 ScreenCoordinates { get; private set; }

        public static void Update(GameTime gameTime)
        {
            var mouseState = Microsoft.Xna.Framework.Input.Mouse.GetState();

            var newScreenCoordinates = new Vector2(mouseState.X, mouseState.Y);
            if (newScreenCoordinates != ScreenCoordinates)
            {
                var handler = MouseMoved;
                if (handler != null) handler(null, new EventArgs());
                ScreenCoordinates = newScreenCoordinates;
            }

            if (!_lmbDown && mouseState.LeftButton == ButtonState.Pressed)
            {
                var handler = LmbDown;
                if (handler != null) handler(null, new EventArgs());

                _lmbDown = true;
            }

            if (_lmbDown && mouseState.LeftButton == ButtonState.Released)
            {
                var handler = LmbUp;
                if (handler != null) handler(null, new EventArgs());

                _lmbDown = false;
            }
        }
    }
}
