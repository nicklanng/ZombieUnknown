using System;
using Microsoft.Xna.Framework;

namespace Engine.Extensions
{
    public static class Vector2Extensions
    {
        public static Vector2 Truncate(this Vector2 vector, float maxLength)
        {
            if (vector.Length() <= maxLength)
            {
                return vector;
            }

            return vector.NormalizeFixed() * maxLength;
        }

        public static float DistanceTo(this Vector2 vector, Vector2 otherVector)
        {
            return Math.Abs((otherVector - vector).Length());
        }

        public static Vector2 NormalizeFixed(this Vector2 vector)
        {
            if (vector == Vector2.Zero) return vector;
            vector.Normalize();
            return vector;
        }

        public static Vector2 Inverse(this Vector2 vector)
        {
            return new Vector2(vector.X * -1, vector.Y * -1);
        }
    }
}
