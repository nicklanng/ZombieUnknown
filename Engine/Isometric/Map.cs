﻿using System;
using System.Collections.Generic;
using Engine.Isometric.Entities;
using Engine.Pathfinding;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Isometric
{
    public class Map
    {
        private readonly short _width;
        private readonly short _height;

        private readonly Tile[,] _tiles;
        private readonly Manual2dCamera _manual2DCamera;

        private readonly List<Light> _lights;

        private readonly Node[,] _nodes;

        private const float TileHeight = 48;
        private const float FloorWidth = 32;
        private const float FloorHeight = 16;

        private Color _ambientLight;

        public Map(short width, short height, Tile[,] tiles, Color ambientLight, Manual2dCamera manual2DCamera)
        {
            _width = width;
            _height = height;
            _tiles = tiles;
            _ambientLight = ambientLight;
            _manual2DCamera = manual2DCamera;

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
                    Vector2 screenCoordinates;
                    var isTileOnScreen = GetScreenCoordinates(new Vector2(x, y), out screenCoordinates);
                    if (!isTileOnScreen)
                    {
                        continue;
                    }

                    _tiles[x, y].DrawWalls(spriteBatch, screenCoordinates);
                    _tiles[x, y].DrawFloor(spriteBatch, screenCoordinates);

                    _tiles[x, y].DrawEntities(spriteBatch, screenCoordinates);
                    
                    if (GameState.Selected != null)
                    {
                        if ((int) GameState.Selected.Parent.Position.X == x &&
                            (int) GameState.Selected.Parent.Position.Y == y)
                        {
                            ResourceManager.SelectMarkerSprite.Draw(spriteBatch, screenCoordinates);
                        }
                    }
                }
            }
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

        public bool GetMapCoordinates(Vector2 screenCoordinates, out Vector2 mapCoordinates)
        {
            const int mouseVerticalOffset = 10;

            var xStripped = (screenCoordinates.X / EngineSettings.ZoomFactor) - (int)(_manual2DCamera.Size.X / 2) + (int)_manual2DCamera.Position.X + FloorWidth / 2;
            var yStripped = ((screenCoordinates.Y / EngineSettings.ZoomFactor) - (int)(_manual2DCamera.Size.Y / 2) + (int)_manual2DCamera.Position.Y) + mouseVerticalOffset;

            var isoX = yStripped / (FloorHeight) + xStripped / (FloorWidth);
            var isoY = -(yStripped / (FloorHeight) - xStripped / (FloorWidth));

            mapCoordinates = new Vector2(isoX, isoY);

            if (isoX < 0 || isoX >= _width || isoY < 0 || isoY >= _height)
            {
                return false;
            }

            return true;
        }

        public MoveableEntity GetSelected(Vector2 mapPosition)
        {
            var tile = _tiles[(int)mapPosition.X, (int)mapPosition.Y];
            return tile.MoveableEntity;
        }

        private bool GetScreenCoordinates(Vector2 mapCoordinates, out Vector2 screenCoordinates)
        {
            screenCoordinates = Vector2.Zero;

            var xCoord = ((mapCoordinates.X + mapCoordinates.Y)*FloorWidth/2) -(int)_manual2DCamera.Position.X + (int)(_manual2DCamera.Size.X / 2);
            if (xCoord <= 0)
            {
                if (xCoord + FloorWidth < 0)
                {
                    return false;
                }
            }
            if (xCoord >= _manual2DCamera.Size.X)
            {
                if (xCoord - FloorWidth > _manual2DCamera.Size.X)
                {
                    return false;
                }
            }

            var yCoord = ((mapCoordinates.X - mapCoordinates.Y)*FloorHeight/2) - (int)_manual2DCamera.Position.Y + (int)(_manual2DCamera.Size.Y / 2);
            if (yCoord <= 0)
            {
                if (yCoord + TileHeight < 0)
                {
                    return false;
                }
            }
            if (yCoord >= _manual2DCamera.Size.Y)
            {
                if (yCoord - TileHeight > _manual2DCamera.Size.Y)
                {
                    return false;
                }
            }
            
            screenCoordinates = new Vector2(xCoord, yCoord);
            return true;
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

                var lightPosition = light.Parent.Position;

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
                var lightPosition = light.Parent.Position;
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

                    //Console.WriteLine(node);
                }
            }
        }

        private bool IsPositionOnMap(int x, int y)
        {
            if (x < 0) return false;
            if (x >= _width) return false;
            if (y < 0) return false;
            if (y >= _height) return false;

            return true;
        }
    }
}
