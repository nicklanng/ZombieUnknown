using Microsoft.Xna.Framework;

namespace Engine.Extensions
{
    public static class RectangleExtensions
    {
        public static int GetArea(this Rectangle rectangle)
        {
            return rectangle.Width*rectangle.Height;
        }
    }
}
