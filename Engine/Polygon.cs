using System;
using System.Text;
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
                if (point.Y >= Math.Min(side.Start.Y, side.End.Y))
                {
                    if (point.Y <= Math.Max(side.Start.Y, side.End.Y))
                    {
                        if (point.X <= Math.Max(side.Start.X, side.End.X))
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

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.AppendLine(GetType() + ":");
            foreach (var line in _lines)
            {
                builder.AppendLine(line.ToString());
            }

            return builder.ToString();
        }
    }
}