using System.Collections.Generic;
using Engine;
using Engine.Maps;
using Microsoft.Xna.Framework;

namespace ZombieUnknown.ProceduralGeneration
{
    class Building
    {
        private readonly Rectangle _totalArea;
        private readonly List<Room> _placedRooms;

        public Building(Rectangle totalArea, List<Room> placedRooms)
        {
            _totalArea = totalArea;
            _placedRooms = placedRooms;
        }

        public void PlaceAt(Coordinate worldPosition)
        {
            var map = GameState.Map;

            foreach (var room in _placedRooms)
            {
                room.DrawWalls(worldPosition);
            }

            // draw external walls
            for (var x = worldPosition.X + _totalArea.X; x < worldPosition.X + _totalArea.X + _totalArea.Width; x++)
            {
                map.GetTile(new Coordinate(x, worldPosition.Y + _totalArea.Y + _totalArea.Height)).SetLeftWall(ResourceManager.GetSprite("urbanExterior001Left"));
            }
            for (var y = worldPosition.Y + _totalArea.Y; y < worldPosition.Y + _totalArea.Y + _totalArea.Height; y++)
            {
                map.GetTile(new Coordinate(worldPosition.X + _totalArea.X + _totalArea.Width, y)).SetRightWall(ResourceManager.GetSprite("urbanExterior001Right"));
            }

            // draw outer Join
            map.GetTile(new Coordinate(worldPosition.X + _totalArea.X + _totalArea.Width, worldPosition.Y + _totalArea.Y + _totalArea.Height)).SetJoinWall(ResourceManager.GetSprite("urbanExterior001ExternalJoin"));

        }
    }
}