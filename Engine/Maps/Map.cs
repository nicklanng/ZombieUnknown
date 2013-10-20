using System;
using System.Collections.Generic;
using Engine.Entities;
using Engine.Maths;
using Engine.Pathfinding;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Maps
{
    public class Map
    {
        private readonly short _width;
        private readonly short _height;

        private readonly Tile[,] _tiles;

        private readonly List<Light> _lights;

        private readonly Node[,] _nodes;

        private Color _ambientLight;

        private readonly ICamera _camera;

        public Map(short width, short height, Tile[,] tiles, Color ambientLight, ICamera camera)
        {
            _width = width;
            _height = height;
            _tiles = tiles;
            _ambientLight = ambientLight;
            _camera = camera;

            _nodes = new Node[width, height];
            for (var x = 0; x < _width; x++)
            {
                for (var y = 0; y < _height; y++)
                {
                    var node = new Node(new Vector2(x, y));
                    _nodes[x, y] = node;
                }
            }
            RegeneratePathfindingMap();

            _lights = new List<Light>();

            RegenerateLightMap();
        }

        public void Update(GameTime gameTime)
        {
            for (var x = 0; x < _width; x++)
            {
                for (var y = 0; y < _height; y++)
                {
                    _tiles[x, y].Update(gameTime);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Cursor cursor)
        {
            for (var x = 0; x < _width; x++)
            {
                for (var y = _height - 1; y >= 0; y--)
                {
                    _tiles[x, y].DrawFloor();

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

        public Node GetNodeAt(Vector2 position)
        {
            return _nodes[(int)position.X, (int)position.Y];
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
            
            _lights.Add(lightEntity);
            RegenerateLightMap();
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

            _lights.Remove(lightEntity);
            RegenerateLightMap();
        }

        public MoveableEntity GetSelected(Vector2 mapPosition)
        {
            var tile = _tiles[(int)mapPosition.X, (int)mapPosition.Y];
            return tile.MoveableEntity;
        }

        private void RegenerateLightMap()
        {
            var lightMap = new byte[_width, _height, 3];

            AddWallsToLight();

            AddLightEntities(lightMap);

            SaveLightMapToTiles(lightMap);
        }

        private void AddWallsToLight()
        {
            foreach (var light in _lights)
            {
                var walls = new List<Line>();

                var lightPosition = light.MapPosition;

                var lightMapSize = light.Range*2 + 1;

                for (var x = 0; x < lightMapSize; x++)
                {
                    var mapX = (int) lightPosition.X - light.Range + x;
                    if (mapX < 0 || mapX >= _width) continue;

                    for (var y = 0; y < lightMapSize; y++)
                    {
                        var mapY = (int)lightPosition.Y - light.Range + y;
                        if (mapY < 0 || mapY >= _height) continue;

                        var tile = _tiles[mapX, mapY];
                        if (tile.HasLeftWall)
                        {
                            walls.Add(new Line(new Vector2(x, y), new Vector2(x, y + 1)));
                        }
                        if (tile.HasRightWall)
                        {
                            walls.Add(new Line(new Vector2(x, y + 1), new Vector2(x + 1, y + 1)));
                        }
                    }
                }

                light.GenerateVisibiltyMap(walls);
            }
        }

        private void AddLightEntities(byte[,,] lightMap)
        {
            foreach (var light in _lights)
            {
                var lightColor = light.Color;
                var lightPosition = light.MapPosition;
                var intensityMap = light.IntensityMap;
                var visibilityMap = light.VisiblityMap;
                var lightMapSize = light.Range*2 + 1;

                for (var x = 0; x < lightMapSize; x++)
                {
                    var mapX = x - light.Range + (int) lightPosition.X;
                    if (mapX < 0 || mapX >= _width) continue;

                    for (var y = 0; y < lightMapSize; y++)
                    {
                        var mapY = y - light.Range + (int) lightPosition.Y;
                        if (mapY < 0 || mapY >= _height) continue;

                        var intensityMapScalar = intensityMap[x, y] * visibilityMap[x, y];
                        var scaledColorR = (byte) (intensityMapScalar*lightColor.R);
                        var scaledColorG = (byte) (intensityMapScalar*lightColor.G);
                        var scaledColorB = (byte) (intensityMapScalar*lightColor.B);

                        if (scaledColorR > lightMap[mapX, mapY, 0])
                        {
                            lightMap[mapX, mapY, 0] = scaledColorR;
                        }

                        if (scaledColorG > lightMap[mapX, mapY, 1])
                        {
                            lightMap[mapX, mapY, 1] = scaledColorG;
                        }

                        if (scaledColorB > lightMap[mapX, mapY, 2])
                        {
                            lightMap[mapX, mapY, 2] = scaledColorB;
                        }
                    }
                }
            }
        }

        private void SaveLightMapToTiles(byte[, ,] lightMap)
        {
            for (var x = 0; x < _width; x++)
            {
                for (var y = 0; y < _height; y++)
                {
                    var r = (byte)Math.Max(lightMap[x, y, 0], _ambientLight.R);
                    var g = (byte)Math.Max(lightMap[x, y, 1], _ambientLight.G);
                    var b = (byte)Math.Max(lightMap[x, y, 2], _ambientLight.B);
                    _tiles[x, y].Light = new Color(r, g, b);
                }
            }
        }

        private void RegeneratePathfindingMap()
        {
            for (var x = 0; x < _width; x++)
            {
                for (var y = 0; y < _height; y++)
                {
                    var node = _nodes[x, y];

                    if (IsPositionOnMap(x - 1, y))
                    {
                        if (!_tiles[x, y].HasLeftWall)
                        {
                            node.AddNeighbor(_nodes[x - 1, y]);
                        }
                    }

                    if (IsPositionOnMap(x, y - 1))
                    {
                        var neighborNode = _nodes[x, y - 1];
                        if (!_tiles[x, y - 1].HasRightWall)
                        {
                            node.AddNeighbor(neighborNode);
                        }
                    }

                    if (IsPositionOnMap(x + 1, y))
                    {
                        var neighborNode = _nodes[x + 1, y];
                        if (!_tiles[x + 1, y].HasLeftWall)
                        {
                            node.AddNeighbor(neighborNode);
                        }
                    }

                    if (IsPositionOnMap(x, y + 1))
                    {
                        if (!_tiles[x, y].HasRightWall)
                        {
                            node.AddNeighbor(_nodes[x, y + 1]);
                        }
                    }
                }
            }
        }

        public bool IsPositionOnMap(int x, int y)
        {
            if (x < 0) return false;
            if (x >= _width) return false;
            if (y < 0) return false;
            if (y >= _height) return false;

            return true;
        }
    }
}
