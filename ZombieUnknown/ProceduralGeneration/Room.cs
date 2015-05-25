using Engine;
using Engine.Entities;
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

            string sprite;

            
            // draw Left Walls
            for (var x = worldPosition.X + Location.X; x < worldPosition.X + Location.X + Location.Width; x++)
            {
                if (Type == RoomType.Bathroom)
                {
                    sprite = "urbanInterior002Left";
                }
                else
                {
                    sprite = "urbanInterior001Left";
                }
                map.GetTile(new Coordinate(x, worldPosition.Y + Location.Y)).SetLeftWall(ResourceManager.GetSprite(sprite));
            }

            // draw Right Walls
            for (var y = worldPosition.Y + Location.Y; y < worldPosition.Y + Location.Y + Location.Height; y++)
            {
                if (Type == RoomType.Bathroom)
                {
                    sprite = "urbanInterior002Right";
                }
                else
                {
                    sprite = "urbanInterior001Right";
                }
                map.GetTile(new Coordinate(worldPosition.X + Location.X, y)).SetRightWall(ResourceManager.GetSprite(sprite));
            }

            // draw innerJoins
            if (Type == RoomType.Bathroom)
            {
                sprite = "urbanInterior002InternalJoin";
            }
            else
            {
                sprite = "urbanInterior001InternalJoin";
            }
            map.GetTile(new Coordinate(worldPosition.X + Location.X, worldPosition.Y + Location.Y)).SetJoinWall(ResourceManager.GetSprite(sprite));

            // draw Floor
            for (var x = worldPosition.X + Location.X; x < worldPosition.X + Location.X + Location.Width; x++)
            {
                for (var y = worldPosition.Y + Location.Y; y < worldPosition.Y + Location.Y + Location.Height; y++)
                {
                    if (Type == RoomType.Kitchen)
                    {
                        sprite = "urbanInterior002";
                    }
                    else if (Type == RoomType.Bathroom)
                    {
                        sprite = "urbanInterior003";
                    }
                    else
                    {
                        sprite = "urbanInterior001";
                    }
                    map.GetTile(new Coordinate(x, y)).SetFloor(ResourceManager.GetSprite(sprite));
                }
            }

            
            // create ceiling light
            var centre = worldPosition + new Coordinate(Location.Center.X, Location.Center.Y);
            if (GameState.RandomNumberGenerator.Next(2) == 1)
            {
                GameController.SpawnEntity(new PhantomLight("light", centre, Color.White, 10));
            }
        }
    }
}
