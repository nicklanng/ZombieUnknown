using System;
using Engine.Entities;
using Engine.Input;
using Engine.Maps;
using Microsoft.Xna.Framework;

namespace Engine
{
    public class Cursor
    {
        private readonly Map _map;
        private readonly ICamera _camera;
        private readonly CursorBackEntity _cursorBackEntity;
        private readonly CursorFrontEntity _cursorFrontEntity;

        private Vector2 _mapPosition;

        public bool IsOnMap { get; private set; }
        public Vector2 MapPosition { get; private set; }

        public Cursor(Map map, ICamera camera, CursorFrontEntity cursorFrontEntity, CursorBackEntity cursorBackEntity)
        {
            _map = map;
            _camera = camera;
            _cursorFrontEntity = cursorFrontEntity;
            _cursorBackEntity = cursorBackEntity;


            Mouse.MouseMoved += MouseOnMouseMoved;
            Mouse.LmbDown += MouseOnLmbDown;
        }

        private void MouseOnMouseMoved(object sender, EventArgs eventArgs)
        {
            var screenCoordinates = Mouse.ScreenCoordinates;

            var newMapPosition = _camera.GetMapCoordinates(screenCoordinates);

            var newIsOnMap = _map.IsPositionOnMap((int)newMapPosition.X, (int)newMapPosition.Y);

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
