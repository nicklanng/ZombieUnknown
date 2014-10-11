using System;
using Engine.Maps;
using Microsoft.Xna.Framework;

namespace Engine.Maths
{
    public static class NumericExtensions
    {
        public static double ToRadians(this int val)
        {
            return (Math.PI / 180) * val;
        }

        public static Vector2 RotateByRadians(this Coordinate vector, double radians)
        {
            var x = (float)(Math.Cos(radians) * vector.X - Math.Sin(radians) * vector.Y);
            var y = (float)(Math.Sin(radians) * vector.X + Math.Cos(radians) * vector.Y);

            return new Vector2(x, y);
        }
    }
}
