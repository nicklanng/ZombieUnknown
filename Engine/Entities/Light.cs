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

        public void UpdateVisibility(Coordinate centerCoordinate)
        {
            var startX = centerCoordinate.X - Range;
            var startY = centerCoordinate.Y - Range;

            var walls = new List<Line>();
            for (var x = 0; x < _mapSize; x++)
            {
                var mapX = startX + x;

                for (var y = 0; y < _mapSize; y++)
                {
                    var mapY = startY + y;

                    var tile = GameState.Map.GetTile(new Coordinate(mapX, mapY));
                    if (tile == null)
                    {
                        continue;
                    }
                    if (tile.HasLeftWall)
                    {
                        var wallLine = new Line(new Vector2(x, y), new Vector2(x + 1, y));
                        var extendingLine = false;
                        for (var index = 0; index < walls.Count; index++)
                        {
                            var wall = walls[index];
                            if (wall.Direction != LineDirection.LeftWall)
                            {
                                continue;
                            }
                            if (Math.Abs(wall.Start.Y - wallLine.End.Y) < 0.001 && Math.Abs(wall.Start.X - wallLine.End.X) < 0.001)
                            {
                                wall.Start = wallLine.Start;
                                extendingLine = true;
                                walls[index] = wall;
                                break;
                            }
                            if (Math.Abs(wall.End.Y - wallLine.Start.Y) < 0.001 && Math.Abs(wall.End.X - wallLine.Start.X) < 0.001)
                            {
                                wall.End = wallLine.End;
                                extendingLine = true;
                                walls[index] = wall;
                                break;
                            }
                        }

                        if (!extendingLine) walls.Add(wallLine);
                    }
                    if (tile.HasRightWall)
                    {
                        var wallLine = new Line(new Vector2(x, y), new Vector2(x, y + 1));
                        var extendingLine = false;
                        for (var index = 0; index < walls.Count; index++)
                        {
                            var wall = walls[index];
                            if (wall.Direction != LineDirection.RightWall)
                            {
                                continue;
                            }
                            if (Math.Abs(wall.Start.Y - wallLine.End.Y) < 0.001 && Math.Abs(wall.Start.X - wallLine.End.X) < 0.001)
                            {
                                wall.End = wallLine.End;
                                extendingLine = true;
                                walls[index] = wall;
                                break;
                            }
                            if (Math.Abs(wall.End.Y - wallLine.Start.Y) < 0.001 && Math.Abs(wall.End.X - wallLine.Start.X) < 0.001)
                            {
                                wall.End = wallLine.End;
                                extendingLine = true;
                                walls[index] = wall;
                                break;
                            }
                        }

                        if (!extendingLine) walls.Add(wallLine);
                    }
                }
            }

            // create shadow polygons
            var shadowPolygons = new List<Polygon>();
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

            // render shadows

            var shadowInc = 1.0f / EngineSettings.ShadowQuality;
            for (var x = 0; x < _mapSize; x++)
            {
                for (var y = 0; y < _mapSize; y++)
                {
                    VisiblityMap[x, y] = 1;

                    var shadowTiles = 0;

                    for (var xInc = shadowInc / 2; xInc < 1.0; xInc += shadowInc)
                    {
                        for (var yInc = shadowInc / 2; yInc < 1.0; yInc += shadowInc)
                        {
                            var point = new Vector2(x + xInc, y + yInc);
                            if (shadowPolygons.Any(sp => sp.PointInPolygon(point)))
                            {
                                shadowTiles++;
                            }
                        }
                    }
                    VisiblityMap[x, y] = 1 - (float)(shadowTiles / Math.Pow(EngineSettings.ShadowQuality, 2));
                }
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
                xCoord = Math.Sign(deltaX) * (Range * 100 + 1) + centerOfLightSquare;
                yCoord = Math.Sign(deltaX) * (Range * 100 + 1) * gradient + centerOfLightSquare;
            }
            else
            {
                var gradient = deltaX / deltaY;
                yCoord = Math.Sign(deltaY) * (Range * 100 + 1) + centerOfLightSquare;
                xCoord = Math.Sign(deltaY) * (Range * 100 + 1) * gradient + centerOfLightSquare;
            }

            return new Vector2(xCoord, yCoord);
        }
    }
}
