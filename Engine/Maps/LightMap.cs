using System;
using Microsoft.Xna.Framework;

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
            
            AddLightEntities();

            SaveLightMapToTiles();
        }

        private void AddLightEntities()
        {
            foreach (var lightsource in _map.Lights)
            {
                var light = lightsource.Light;

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
                    var r = Math.Max(_lightValues[x, y, 0], _ambientLight.R);
                    var g = Math.Max(_lightValues[x, y, 1], _ambientLight.G);
                    var b = Math.Max(_lightValues[x, y, 2], _ambientLight.B);

                    _map.GetTile(new Coordinate(x, y)).Light = new Color(r, g, b);
                }
            }
        }
    }
}
