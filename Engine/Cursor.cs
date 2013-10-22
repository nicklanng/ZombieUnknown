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

        private Coordinate _coordinate;

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

            var newMapPosition = Coordinate.FromVector2(_camera.GetMapCoordinates(screenCoordinates));

            var newIsOnMap = _map.IsPositionOnMap(newMapPosition);

            if (newMapPosition == _coordinate)
            {
                return;
            }

            if (IsOnMap)
            {
                _map.RemoveEntity(_cursorBackEntity.Coordinate, _cursorBackEntity);
                _map.RemoveEntity(_cursorFrontEntity.Coordinate, _cursorFrontEntity);
            }

            IsOnMap = newIsOnMap;
            _coordinate = newMapPosition;

            _cursorBackEntity.Coordinate = newMapPosition;
            _cursorFrontEntity.Coordinate = newMapPosition;

            if (IsOnMap)
            {
                _map.AddEntity(_cursorBackEntity.Coordinate, _cursorBackEntity);
                _map.AddEntity(_cursorFrontEntity.Coordinate, _cursorFrontEntity);
            }
        }

        private void MouseOnLmbDown(object sender, EventArgs eventArgs)
        {
            if (IsOnMap)
            {
                var selected = _map.GetSelected(_coordinate);
                GameState.Selected = selected;
            }
            else
            {
                GameState.Selected = null;
            }
        }
    }
}
