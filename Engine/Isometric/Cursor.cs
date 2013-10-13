using System;
using Engine.Input;
using Engine.Isometric.Entities;
using Engine.Sprites;
using Microsoft.Xna.Framework;

namespace Engine.Isometric
{
    public class Cursor
    {
        private readonly Map _map;
        private readonly CursorBackEntity _cursorBackEntity;
        private readonly CursorFrontEntity _cursorFrontEntity;

        private Vector2 _mapPosition;

        public bool IsOnMap { get; private set; }
        public Vector2 MapPosition { get; private set; }

        public Cursor(Map map, Sprite frontCursor, Sprite backCursor)
        {
            _map = map;

            _cursorBackEntity = new CursorBackEntity("CursorBackEntity", backCursor);
            _cursorFrontEntity = new CursorFrontEntity("CursorFrontEntity", frontCursor);

            Mouse.MouseMoved += MouseOnMouseMoved;
            Mouse.LmbDown += MouseOnLmbDown;
        }

        private void MouseOnMouseMoved(object sender, EventArgs eventArgs)
        {
            var screenCoordinates = Mouse.ScreenCoordinates;

            Vector2 newMapPosition;
            var newIsOnMap = _map.GetMapCoordinates(screenCoordinates, out newMapPosition);

            if (newMapPosition == _mapPosition)
            {
                return;
            }

            if (IsOnMap)
            {
                _map.RemoveEntity(_mapPosition, _cursorBackEntity);
                _map.RemoveEntity(_mapPosition, _cursorFrontEntity);
            }

            IsOnMap = newIsOnMap;
            _mapPosition = newMapPosition;

            if (IsOnMap)
            {
                _map.AddEntity(_mapPosition, _cursorBackEntity);
                _map.AddEntity(_mapPosition, _cursorFrontEntity);
            }
        }

        private void MouseOnLmbDown(object sender, EventArgs eventArgs)
        {
            if (IsOnMap)
            {
                var selected = _map.GetSelected(_mapPosition);
                GameState.Selected = selected;
            }
            else
            {
                GameState.Selected = null;
            }
        }
    }
}
