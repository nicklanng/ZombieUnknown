using System;
using Engine.Extensions;
using Microsoft.Xna.Framework;

namespace Engine.Maths
{
    public enum LineDirection
    {
        LeftWall,
        RightWall
    }

    public struct Line
    {
        public LineDirection Direction { get; private set; }
        public Vector2 Start { get; set; }
        public Vector2 End { get; set; }
        public Vector2 Normal { get; private set; }

        public Line(Vector2 start, Vector2 end) 
            : this()
        {
            Start = start;
            End = end;

            Direction = Math.Abs(Start.X - end.X) < 0.001 ? LineDirection.RightWall : LineDirection.LeftWall;

            var wallVector = new Vector2((End.X - Start.X), (End.Y - Start.Y)).NormalizeFixed();
            Normal = Vector2.Transform(wallVector, Matrix.CreateRotationZ(MathHelper.ToRadians(90)));
        }

        public bool IntersectsLine(Line line, out float r)
        {
            r = 0;
            var denominator = ((End.X - Start.X) * (line.End.Y - line.Start.Y)) - ((End.Y - Start.Y) * (line.End.X - line.Start.X));
            var numerator1 = ((Start.Y - line.Start.Y) * (line.End.X - line.Start.X)) - ((Start.X - line.Start.X) * (line.End.Y - line.Start.Y));
            var numerator2 = ((Start.Y - line.Start.Y) * (End.X - Start.X)) - ((Start.X - line.Start.X) * (End.Y - Start.Y));

            // Detect coincident lines (has a problem, read below)
            if (Math.Abs(denominator) < 0.001) return Math.Abs(numerator1) < 0.001 && Math.Abs(numerator2) < 0.001;

            r = numerator1 / denominator;
            var s = numerator2 / denominator;

            return (r >= 0 && r <= 1) && (s >= 0 && s <= 1);
        }

        public override string ToString()
        {
            return string.Format("Line: ({0}, {1}) - ({2}, {3})", Start.X, Start.Y, End.X, End.Y);
        }
    }
}
