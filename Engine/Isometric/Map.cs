using System;
using System.Collections.Generic;
using Engine.Entities;
using Engine.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Isometric
{
    public class Map
    {
        private readonly short _width;
        private readonly short _height;

        private readonly Tile[,] _tiles;
        private readonly Camera _camera;

        private readonly List<Light> _lights; 

        private const float TileWidth = 32;
        private const float TileHeight = 16;

        private Color _ambientLight;

        public Map(short width, short height, Tile[,] tiles, Color ambientLight, Camera camera)
        {
            _width = width;
            _height = height;
            _tiles = tiles;
            _ambientLight = ambientLight;
            _camera = camera;

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
                    var isTileOnScreen = GetScreenCoordinates(new Vector2(x, y), _tiles[x,y].FloorSprite, out screenCoordinates);
                    if (!isTileOnScreen)
                    {
                        continue;
                    }

                    var isCursorHere = cursor.IsOnMap && (int)cursor.MapPosition.X == x && (int)cursor.MapPosition.Y == y;

                    _tiles[x, y].DrawWalls(spriteBatch, screenCoordinates);

                    if (isCursorHere)
                    {
                        cursor.DrawBackSprite(spriteBatch, screenCoordinates);
                    }

                    _tiles[x, y].DrawFloor(spriteBatch, screenCoordinates);

                    _tiles[x, y].DrawEntities(spriteBatch, screenCoordinates);
                    
                    if (isCursorHere)
                    {
                        cursor.DrawFrontSprite(spriteBatch, screenCoordinates);
                    }

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

            if (entity is Light)
            {
                _lights.Add(entity as Light);
                RegenerateLightMap();
            }
        }

        public bool GetMapCoordinates(Vector2 screenCoordinates, out Vector2 mapCoordinates)
        {
            const int mouseVerticalOffset = 10;

            var xStripped = (screenCoordinates.X / EngineSettings.ZoomFactor) - (int)(_camera.Size.X / 2) + (int)_camera.Position.X + TileWidth / 2;
            var yStripped = ((screenCoordinates.Y / EngineSettings.ZoomFactor) - (int)(_camera.Size.Y / 2) + (int)_camera.Position.Y) + mouseVerticalOffset;

            var isoX = yStripped / (TileHeight) + xStripped / (TileWidth);
            var isoY = -(yStripped / (TileHeight) - xStripped / (TileWidth));

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

        private bool GetScreenCoordinates(Vector2 mapCoordinates, Sprite tileSprite, out Vector2 screenCoordinates)
        {
            screenCoordinates = Vector2.Zero;

            var xCoord = ((mapCoordinates.X + mapCoordinates.Y)*TileWidth/2) -(int)_camera.Position.X + (int)(_camera.Size.X / 2);
            if (xCoord <= 0)
            {
                if (xCoord + tileSprite.Width < 0)
                {
                    return false;
                }
            }
            if (xCoord >= _camera.Size.X)
            {
                if (xCoord - tileSprite.Width > _camera.Size.X)
                {
                    return false;
                }
            }

            var yCoord = ((mapCoordinates.X - mapCoordinates.Y)*TileHeight/2) - (int)_camera.Position.Y + (int)(_camera.Size.Y / 2);
            if (yCoord <= 0)
            {
                if (yCoord + tileSprite.Height < 0)
                {
                    return false;
                }
            }
            if (yCoord >= _camera.Size.Y)
            {
                if (yCoord - tileSprite.Height > _camera.Size.Y)
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
                        if (tile.LeftWallSprite != null)
                        {
                            walls.Add(new Line(new Vector2(x, y), new Vector2(x, y + 1)));
                        }
                        if (tile.RightWallSprite != null)
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
                    _tiles[x, y].SetLight(new Color(r, g, b));
                }
            }
        }
    }
}
