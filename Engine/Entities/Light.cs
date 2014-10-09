using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Maps;
using Engine.Maths;
using Microsoft.Xna.Framework;

namespace Engine.Entities
{
    public class Light
    {
        private readonly int _mapSize;

        public Coordinate Coordinate { get; set; }

        public Color Color { get; private set; }

        public short Range { get; private set; }

        public float[,] IntensityMap { get; private set; }

        public float[,] VisiblityMap { get; private set; }

        public Light(Coordinate coordinate, Color color, short range)
        {
            Coordinate = coordinate;
            Color = color;
            Range = range;

            _mapSize = 2 * Range + 1;

            IntensityMap = new float[_mapSize, _mapSize];
            VisiblityMap = new float[_mapSize, _mapSize];

            GenerateIntensityMap();
        }

        public void GenerateVisibiltyMap(List<Line> walls)
        {
            var shadowInc = 1.0f / EngineSettings.ShadowQuality;
            var shadowPolygons = new List<Polygon>();

            CreateLineOfSightMasks(walls, shadowPolygons);

            for (var x = 0; x < _mapSize; x++)
            {
                for (var y = 0; y < _mapSize; y++)
                {
                    VisiblityMap[x, y] = 1;

                    var litTiles = 0;

                    for (var xInc = shadowInc / 2; xInc < 1.0; xInc += shadowInc)
                    {
                        for (var yInc = shadowInc / 2; yInc < 1.0; yInc += shadowInc)
                        {
                            var point = new Vector2(x + xInc, y + yInc);
                            if (shadowPolygons.Any(sp => sp.PointInPolygon(point)))
                            {
                                litTiles++;
                            }
                        }
                    }
                    VisiblityMap[x, y] = 1 - (float)(litTiles / Math.Pow(EngineSettings.ShadowQuality, 2));
                }
            }
        }

        private void CreateLineOfSightMasks(IEnumerable<Line> walls, ICollection<Polygon> shadowPolygons)
        {
            foreach (var wall in walls)
            {
                var startExtrapolation = GetPointExtrapolation(wall.Start);
                var endExtrapolation = GetPointExtrapolation(wall.End);

                var shadowPolygon = new Polygon(new List<Line>
                {
                    new Line(wall.Start, wall.End),
                    new Line(wall.Start, startExtrapolation),
                    new Line(wall.End, endExtrapolation),
                    new Line(startExtrapolation, endExtrapolation)
                });

                shadowPolygons.Add(shadowPolygon);
            }
        }

        private Vector2 GetPointExtrapolation(Vector2 point)
        {
            var centerOfLightSquare = Range + 0.5f;
            var deltaX = point.X - centerOfLightSquare;
            var deltaY = point.Y - centerOfLightSquare;

            float xCoord, yCoord;
            if (Math.Abs(deltaX) > Math.Abs(deltaY))
            {
                var gradient = deltaY / deltaX;
                xCoord = Math.Sign(deltaX) * Range + centerOfLightSquare -1;
                yCoord = Math.Sign(deltaX) * Range * gradient + centerOfLightSquare -1;
            }
            else
            {
                var gradient = deltaX / deltaY;
                yCoord = Math.Sign(deltaY) * Range + centerOfLightSquare - 1;
                xCoord = Math.Sign(deltaY) * Range * gradient + centerOfLightSquare - 1;
            }

            return new Vector2(xCoord, yCoord);
        }

        private void GenerateIntensityMap()
        {
            var source = new Vector2(Range, Range);

            for (var x = 0; x < _mapSize; x++)
            {
                for (var y = 0; y < _mapSize; y++)
                {
                    var tile = new Vector2(x, y);

                    if (tile == source)
                    {
                        IntensityMap[x, y] = 1;
                    }
                    else
                    {
                        var distance = Vector2.Distance(source, tile);

                        var scalar = 1 - (distance / Range);
                        if (scalar < 0) scalar = 0;

                        IntensityMap[x, y] = scalar;
                    }
                }
            }
        }
    }
}
