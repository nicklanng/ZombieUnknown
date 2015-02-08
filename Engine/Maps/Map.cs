using System;
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
        private readonly List<PhysicalEntity> _entities;

        public List<ILightSource> Lights { get; private set; }
        public short Width { get; private set; }
        public short Height { get; private set; }

        public Map(short width, short height, Tile[,] tiles)
        {
            Width = width;
            Height = height;

            _tiles = tiles;
            _entities = new List<PhysicalEntity>();
            Lights = new List<ILightSource>();
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

            for (var i = 0; i < _entities.Count; i++)
            {
                _entities[i].Update();
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

        public void AddEntity(PhysicalEntity entity)
        {
            _entities.Add(entity);

            var lightEntity = entity as ILightSource;
            if (lightEntity == null)
            {
                return;
            }
            
            Lights.Add(lightEntity);
        }

        public void RemoveEntity(PhysicalEntity entity)
        {
            _entities.Remove (entity);

            var lightEntity = entity as ILightSource;
            if (lightEntity == null) {
                return;
            }

            Lights.Remove (lightEntity);
        }

        public List<PhysicalEntity> GetEntitiesAt(Vector2 mapPosition) 
        {
            return _entities.Where(x => x.MapPosition == mapPosition).ToList();
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
                }
            }

            foreach (var drawableEntity in _entities.OfType<IDrawingProvider>())
            {
                drawingRequests.AddRange(drawableEntity.GetDrawings());
            }

            return drawingRequests;
        }
    }
}
