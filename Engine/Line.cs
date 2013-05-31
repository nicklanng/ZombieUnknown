using Microsoft.Xna.Framework;

namespace Engine
{
    public struct Line
    {
        public Vector2 Start { get; private set; }
        public Vector2 End { get; private set; }

        public Line(Vector2 start, Vector2 end) 
            : this()
        {
            Start = start;
            End = end;
        }
    }
}
