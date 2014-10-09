using System;
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

        public Line(Vector2 start, Vector2 end) 
            : this()
        {
            Start = start;
            End = end;

            Direction = Math.Abs(Start.X - end.X) < 0.001 ? LineDirection.RightWall : LineDirection.LeftWall;
        }

        public override string ToString()
        {
            return string.Format("Line: ({0}, {1}) - ({2}, {3})", Start.X, Start.Y, End.X, End.Y);
        }
    }
}
