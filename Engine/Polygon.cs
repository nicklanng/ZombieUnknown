using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Engine
{
    public class Polygon
    {
        private readonly List<Line> _lines;

        public Polygon(List<Line> lines)
        {
            _lines = lines;
        }

        public bool PointInPolygon(Vector2 point) 
        {
            var inside = false;

            foreach (var side in _lines) 
            {
                if (point.Y > Math.Min((float)side.Start.Y, (float)side.End.Y))
                {
                    if (point.Y <= Math.Max((float)side.Start.Y, (float)side.End.Y))
                    {
                        if (point.X <= Math.Max((float)side.Start.X, (float)side.End.X))
                        {
                            float xIntersection = side.Start.X + ((point.Y - side.Start.Y) / (side.End.Y - side.Start.Y)) * (side.End.X - side.Start.X);
                            if (point.X <= xIntersection)
                            {
                                inside = !inside;
                            }
                        }
                    }
                }
            }

            return inside;
        }
    }
}