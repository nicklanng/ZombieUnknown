using System;
using System.Collections.Generic;
using Engine.Drawing;
using Engine.Drawing.UI;
using Engine.Maps;
using Engine.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Engine.Input
{
    public class Mouse : IUIProvider
    {
        private bool _lmbDown;
        private bool _rmbDown;

        public event EventHandler MouseMoved;
        public event EventHandler LmbDown;
        public event EventHandler LmbUp;
        public event EventHandler RmbDown;
        public event EventHandler RmbUp;

        private Mouse() { }
        public static readonly Mouse Instance = new Mouse();
        private Sprite _sprite;

        public Vector2 ScreenCoordinates { get; private set; }

        public void Initialize()
        {
            _sprite = ResourceManager.GetSprite("cursor");
        }

        public void Update()
        {
            var mouseState = Microsoft.Xna.Framework.Input.Mouse.GetState();

            var newScreenCoordinates = new Vector2(mouseState.X, mouseState.Y);
            if (newScreenCoordinates != ScreenCoordinates)
            {
                var handler = MouseMoved;
                if (handler != null) handler(null, new MouseArgs(newScreenCoordinates));
                ScreenCoordinates = newScreenCoordinates;
            }

            if (!_lmbDown && mouseState.LeftButton == ButtonState.Pressed)
            {
                var handler = LmbDown;
                if (handler != null) handler(null, new MouseArgs(newScreenCoordinates));

                _lmbDown = true;
            }

            if (_lmbDown && mouseState.LeftButton == ButtonState.Released)
            {
                var handler = LmbUp;
                if (handler != null) handler(null, new MouseArgs(newScreenCoordinates));

                _lmbDown = false;
            }

            if (!_rmbDown && mouseState.RightButton == ButtonState.Pressed)
            {
                var handler = RmbDown;
                if (handler != null) handler(null, new MouseArgs(newScreenCoordinates));

                _rmbDown = true;
            }

            if (_rmbDown && mouseState.RightButton == ButtonState.Released)
            {
                var handler = RmbUp;
                if (handler != null) handler(null, new MouseArgs(newScreenCoordinates));

                _rmbDown = false;
            }
        }

        public IEnumerable<UIRequest> GetDrawings()
        {
            var virtualScreenCoordinates = GameState.VirtualScreen.ConvertScreenCoordinatesToVirtualScreenCoordinates(ScreenCoordinates);
            return new[] { new UIRequest(_sprite, new Coordinate((int)virtualScreenCoordinates.X, (int)virtualScreenCoordinates.Y), 1.0f) };
        }

        public static IUIProvider DrawingProvider
        {
            get { return Instance; }
        }
    }
}
