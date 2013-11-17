using System.Collections.Generic;
using System.Linq;
using Engine.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Maps
{
    public class Map
    {
        private readonly Tile[,] _tiles;
        private readonly List<Entity>[,] _entities;

        public List<Light> Lights { get; private set; }

        public short Width { get; private set; }

        public short Height { get; private set; }

        public Map(short width, short height, Tile[,] tiles)
        {
            Width = width;
            Height = height;

            _tiles = tiles;
            _entities = new List<Entity>[Width, Height];
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    _entities[x, y] = new List<Entity>();
                }
            }

            Lights = new List<Light>();
        }

        public void Update(GameTime gameTime)
        {
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    _tiles[x, y].Update(gameTime);
                    _entities[x, y].ForEach(e => e.Update(gameTime));
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

                    var list = _entities[x, y].OrderBy(e => e.ZIndex);
                    foreach (var entity in list)
                    {
                        entity.Draw(_tiles[x, y].Light);
                    }

                    //_tiles[x, y].DrawEntities();

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

        public Tile GetTile(Coordinate coordinate) 
        {
            if (!IsPositionOnMap(coordinate))
            {
                return null;
            }
            return _tiles[coordinate.X, coordinate.Y];
        }

        public void AddEntity(Entity entity, Coordinate coordinate)
        {
            _entities[coordinate.X, coordinate.Y].Add(entity);

            var lightEntity = entity as Light;
            if (lightEntity == null)
            {
                return;
            }
            
            Lights.Add(lightEntity);
        }

        public void RemoveEntity(Entity entity)
        {
            _entities[entity.Coordinate.X, entity.Coordinate.Y].Remove(entity);

            var lightEntity = entity as Light;
            if (lightEntity == null)
            {
                return;
            }

            Lights.Remove(lightEntity);
        }

        public MoveableEntity GetSelected(Coordinate coordinate)
        {
            var entities = _entities[coordinate.X, coordinate.Y];
            return (MoveableEntity)entities.FirstOrDefault(e => e is MoveableEntity);
        }

        public bool IsPositionOnMap(Coordinate coordinate)
        {
            if (coordinate.X < 0) return false;
            if (coordinate.X >= Width) return false;
            if (coordinate.Y < 0) return false;
            if (coordinate.Y >= Height) return false;

            return true;
        }
    }
}
