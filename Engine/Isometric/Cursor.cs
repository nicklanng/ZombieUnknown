using System;
using Engine.Input;
using Engine.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Isometric
{
    public class Cursor
    {
        private readonly Map _map;
        private readonly Sprite _frontCursor;
        private readonly Sprite _backCursor;

        public bool IsOnMap { get; private set; }
        public Vector2 MapPosition { get; private set; }

        public Cursor(Map map, Sprite frontCursor, Sprite backCursor)
        {
            _map = map;
            _frontCursor = frontCursor;
            _backCursor = backCursor;

            Mouse.MouseMoved += MouseOnMouseMoved;
            Mouse.LmbDown += MouseOnLmbDown;
        }

        private void MouseOnMouseMoved(object sender, EventArgs eventArgs)
        {
            var screenCoordinates = Mouse.ScreenCoordinates;

            Vector2 mapPosition;
            IsOnMap = _map.GetMapCoordinates(screenCoordinates, out mapPosition);
            MapPosition = mapPosition;
        }

        private void MouseOnLmbDown(object sender, EventArgs eventArgs)
        {
            if (IsOnMap)
            {
                var selected = _map.GetSelected(MapPosition);
                GameState.Selected = selected;
            }
            else
            {
                GameState.Selected = null;
            }
        }

        public void DrawFrontSprite(SpriteBatch spriteBatch, Vector2 position)
        {
            _frontCursor.Draw(spriteBatch, position);
        }

        public void DrawBackSprite(SpriteBatch spriteBatch, Vector2 position)
        {
            _backCursor.Draw(spriteBatch, position);
        }
    }
}
