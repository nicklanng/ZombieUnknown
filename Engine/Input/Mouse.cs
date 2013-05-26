using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Engine.Input
{
    public static class Mouse
    {
        private static bool _lmbDown;

        public static void Update(GameTime gameTime)
        {
            var mouseState = Microsoft.Xna.Framework.Input.Mouse.GetState();

            if (!_lmbDown && mouseState.LeftButton == ButtonState.Pressed)
            {
                _lmbDown = true;
            }

            if (_lmbDown && mouseState.LeftButton == ButtonState.Released)
            {
                _lmbDown = false;
            }
        }
    }
}
