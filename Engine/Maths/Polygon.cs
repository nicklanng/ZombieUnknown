using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Engine.Maths
{
    public class Polygon
    {
        private readonly List<Line> _lines;
        private float _minX;
        private float _minY;
        private float _maxX;
        private float _maxY;

        public Polygon(List<Line> lines)
        {
            _lines = lines;

            CalculateBounds();
        }

        public bool PointInPolygon(Vector2 point) 
        {
            if (point.Y < _minY)
            {
                return false;
            }
            if (point.Y > _maxY)
            {
                return false;
            }

            if (point.X < _minX)
            {
                return false;
            }
            if (point.X > _maxX)
            {
                return false;
            }


            var inside = false;
            foreach (var side in _lines) 
            {
                if (DoLinesIntersect(new Line(new Vector2(_minX - 1, _minY - 1), point), side))
                {
                    inside = !inside;
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

        private static bool DoLinesIntersect(Line line1, Line line2)
        {
            // http://stackoverflow.com/questions/563198/how-do-you-detect-where-two-line-segments-intersect

            float s1_x, s1_y, s2_x, s2_y;
            s1_x = line1.End.X - line1.Start.X; s1_y = line1.End.Y - line1.Start.Y;
            s2_x = line2.End.X - line2.Start.X; s2_y = line2.End.Y - line2.Start.Y;

            float s, t;
            s = (-s1_y * (line1.Start.X - line2.Start.X) + s1_x * (line1.Start.Y - line2.Start.Y)) / (-s2_x * s1_y + s1_x * s2_y);
            t = (s2_x * (line1.Start.Y - line2.Start.Y) - s2_y * (line1.Start.X - line2.Start.X)) / (-s2_x * s1_y + s1_x * s2_y);

            if (s >= 0 && s <= 1 && t >= 0 && t <= 1)
            {
                return true;
            }

            return false; // No collision

        }

        private void CalculateBounds()
        {
            _minX = Math.Min(_lines.Min(l => l.Start.X), _lines.Min(l => l.End.X));
            _minY = Math.Min(_lines.Min(l => l.Start.Y), _lines.Min(l => l.End.Y));
            _maxX = Math.Max(_lines.Max(l => l.Start.X), _lines.Max(l => l.End.X));
            _maxY = Math.Max(_lines.Max(l => l.Start.Y), _lines.Max(l => l.End.Y));
        }
    }
}