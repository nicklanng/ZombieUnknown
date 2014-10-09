using System.Collections.Generic;
using System.Linq;
using Engine.Drawing;
using Engine.Entities;
using Microsoft.Xna.Framework;

namespace Engine.Maps
{
    public class Map : IDrawingProvider
    {
        private readonly Tile[,] _tiles;
        private readonly List<Entity>[,] _entities;

        public List<ILightSource> Lights { get; private set; }
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

            Lights = new List<ILightSource>();
        }

        public void Update(GameTime gameTime)
        {
            for (var x = 0; x < Width; x++)
            {
                var entitiesToUpdate = new List<Entity>();
                for (var y = 0; y < Height; y++)
                {
                    _tiles[x, y].Update(gameTime);
                    _entities[x, y].ForEach(entitiesToUpdate.Add);
                }
                entitiesToUpdate.ForEach(e => e.Update(gameTime));
            }
        }

        public Tile GetTile(Coordinate coordinate)
        {
            return IsPositionOnMap(coordinate) ? _tiles[coordinate.X, coordinate.Y] : null;
        }
        
        public IEnumerable<Entity> GetEntities(Coordinate coordinate)
        {
            return IsPositionOnMap(coordinate) ? _entities[coordinate.X, coordinate.Y] : new List<Entity>();
        }

        public void AddEntity(Entity entity)
        {
            _entities[entity.GetCoordinate().X, entity.GetCoordinate().Y].Add(entity);

            var lightEntity = entity as ILightSource;
            if (lightEntity == null)
            {
                return;
            }
            
            Lights.Add(lightEntity);
        }

        public void RemoveEntity(Entity entity)
        {
            _entities[entity.GetCoordinate().X, entity.GetCoordinate().Y].Remove(entity);

            var lightEntity = entity as ILightSource;
            if (lightEntity == null)
            {
                return;
            }

            Lights.Remove(lightEntity);
        }

        public DrawableEntity GetSelected(Coordinate coordinate)
        {
            var entities = _entities[coordinate.X, coordinate.Y];
            return (DrawableEntity)entities.FirstOrDefault(e => e is DrawableEntity);
        }

        public bool IsPositionOnMap(Coordinate coordinate)
        {
            if (coordinate.X < 0) return false;
            if (coordinate.X >= Width) return false;
            if (coordinate.Y < 0) return false;
            if (coordinate.Y >= Height) return false;

            return true;
        }

        public IEnumerable<DrawingRequest> GetDrawings()
        {
            var drawingRequests = new List<DrawingRequest>();

            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    drawingRequests.AddRange(_tiles[x, y].GetDrawings());
                    foreach (var drawableEntity in _entities[x, y].OfType<DrawableEntity>())
                    {
                        drawingRequests.AddRange(drawableEntity.GetDrawings());
                    }
                }
            }

            return drawingRequests;
        }
    }
}
