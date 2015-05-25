using Engine;
using Engine.Maps;
using Microsoft.Xna.Framework;

namespace ZombieUnknown.ProceduralGeneration
{
    class Room
    {
        public RoomType Type;
        public Rectangle Location;

        public Room(RoomType type, Rectangle location)
        {
            Type = type;
            Location = location;
        }

        public void DrawWalls(Coordinate worldPosition)
        {
            var map = GameState.Map;

            // draw Left Walls
            for (var x = worldPosition.X + Location.X; x < worldPosition.X + Location.X + Location.Width; x++)
            {
                map.GetTile(new Coordinate(x, worldPosition.Y + Location.Y)).SetLeftWall(ResourceManager.GetSprite("urbanLeftWall"));
            }

            // draw Right Walls
            for (var y = worldPosition.Y + Location.Y; y < worldPosition.Y + Location.Y + Location.Height; y++)
            {
                map.GetTile(new Coordinate(worldPosition.X + Location.X, y)).SetRightWall(ResourceManager.GetSprite("urbanRightWall"));
            }

            // draw innerJoins
            map.GetTile(new Coordinate(worldPosition.X + Location.X, worldPosition.Y + Location.Y)).SetJoinWall(ResourceManager.GetSprite("urbanInternalJoin"));
        }
    }
}
