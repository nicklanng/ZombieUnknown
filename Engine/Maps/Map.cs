using System;
using System.Collections.Generic;
using Engine.Entities;
using Engine.Maths;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Maps
{
    public class Map
    {
        private readonly Tile[,] _tiles;

        public List<Light> Lights { get; private set; }

        public short Width { get; private set; }

        public short Height { get; private set; }

        public Map(short width, short height, Tile[,] tiles)
        {
            Width = width;
            Height = height;

            _tiles = tiles;

            Lights = new List<Light>();
        }

        public void Update(GameTime gameTime)
        {
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    _tiles[x, y].Update(gameTime);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Cursor cursor)
        {
            for (var x = 0; x < Width; x++)
            {
                for (var y = Height - 1; y >= 0; y--)
                {
                    _tiles[x, y].DrawFloor();
                }
            }

            for (var x = 0; x < Width; x++)
            {
                for (var y = Height - 1; y >= 0; y--)
                {

                    _tiles[x, y].DrawWalls();

                    _tiles[x, y].DrawEntities();
                    
                    //if (GameState.Selected != null)
                    //{
                    //    if ((int) GameState.Selected.MapPosition.X == x &&
                    //        (int)GameState.Selected.MapPosition.Y == y)
                    //    {
                    //        ResourceManager.SelectMarkerSprite.Draw(spriteBatch);
                    //    }
                    //}
                }
            }
        }

        public Tile GetTile(int x, int y) 
        {
            return _tiles[x, y];
        }

        public void AddEntity(Vector2 position, Entity entity)
        {
            var parentTile = _tiles[(int)position.X, (int)position.Y];
            parentTile.AddEntity(entity);

            var lightEntity = entity as Light;
            if (lightEntity == null)
            {
                return;
            }
            
            Lights.Add(lightEntity);
        }

        public void RemoveEntity(Vector2 position, Entity entity)
        {
            var parentTile = _tiles[(int)position.X, (int)position.Y];
            parentTile.RemoveEntity(entity);

            var lightEntity = entity as Light;
            if (lightEntity == null)
            {
                return;
            }

            Lights.Remove(lightEntity);
        }

        public MoveableEntity GetSelected(Vector2 mapPosition)
        {
            var tile = _tiles[(int)mapPosition.X, (int)mapPosition.Y];
            return tile.MoveableEntity;
        }

        public bool IsPositionOnMap(int x, int y)
        {
            if (x < 0) return false;
            if (x >= Width) return false;
            if (y < 0) return false;
            if (y >= Height) return false;

            return true;
        }
    }
}
