using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Engine.Maths;

namespace Engine.Maps
{
    public class LightMap
    {
        private Map _map;

        private Color _ambientLight;

        private readonly byte[, ,] _lightValues;

        public LightMap(Map map, Color ambientLight)
        {
            _map = map;
            _ambientLight = ambientLight;

            _lightValues = new byte[map.Width, map.Height, 3];

            AddWallsToLight();

            AddLightEntities();

            SaveLightMapToTiles();
        }

        private void AddWallsToLight()
        {
            foreach (var light in _map.Lights)
            {
                var walls = new List<Line>();

                var lightPosition = light.Coordinate;

                var lightMapSize = light.Range * 2 + 1;

                for (var x = 0; x < lightMapSize; x++)
                {
                    var mapX = lightPosition.X - light.Range + x;
                    if (mapX < 0 || mapX >= _map.Width) continue;

                    for (var y = 0; y < lightMapSize; y++)
                    {
                        var mapY = lightPosition.Y - light.Range + y;
                        if (mapY < 0 || mapY >= _map.Height) continue;

                        var tile = _map.GetTile(mapX, mapY);
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

        private void AddLightEntities()
        {
            foreach (var light in _map.Lights)
            {
                var lightColor = light.Color;
                var lightPosition = light.Coordinate;
                var intensityMap = light.IntensityMap;
                var visibilityMap = light.VisiblityMap;
                var lightMapSize = light.Range * 2 + 1;

                for (var x = 0; x < lightMapSize; x++)
                {
                    var mapX = x - light.Range + lightPosition.X;
                    if (mapX < 0 || mapX >= _map.Width) continue;

                    for (var y = 0; y < lightMapSize; y++)
                    {
                        var mapY = y - light.Range + lightPosition.Y;
                        if (mapY < 0 || mapY >= _map.Height) continue;

                        var intensityMapScalar = intensityMap[x, y] * visibilityMap[x, y];
                        var scaledColorR = (byte)(intensityMapScalar * lightColor.R);
                        var scaledColorG = (byte)(intensityMapScalar * lightColor.G);
                        var scaledColorB = (byte)(intensityMapScalar * lightColor.B);

                        if (scaledColorR > _lightValues[mapX, mapY, 0])
                        {
                            _lightValues[mapX, mapY, 0] = scaledColorR;
                        }

                        if (scaledColorG > _lightValues[mapX, mapY, 1])
                        {
                            _lightValues[mapX, mapY, 1] = scaledColorG;
                        }

                        if (scaledColorB > _lightValues[mapX, mapY, 2])
                        {
                            _lightValues[mapX, mapY, 2] = scaledColorB;
                        }
                    }
                }
            }
        }

        private void SaveLightMapToTiles()
        {
            for (var x = 0; x < _map.Width; x++)
            {
                for (var y = 0; y < _map.Height; y++)
                {
                    var r = (byte)Math.Max(_lightValues[x, y, 0], _ambientLight.R);
                    var g = (byte)Math.Max(_lightValues[x, y, 1], _ambientLight.G);
                    var b = (byte)Math.Max(_lightValues[x, y, 2], _ambientLight.B);

                    _map.GetTile(x, y).Light = new Color(r, g, b);
                }
            }
        }
    }
}
