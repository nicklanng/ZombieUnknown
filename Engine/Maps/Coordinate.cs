using System;
using Microsoft.Xna.Framework;

namespace Engine.Maps
{
    public class Coordinate : IEquatable<Coordinate>
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool Equals(Coordinate other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Coordinate && Equals((Coordinate)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }

        public static bool operator ==(Coordinate a, Coordinate b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Coordinate a, Coordinate b)
        {
            return !(a == b);
        }

        public static Coordinate operator +(Coordinate a, Coordinate b)
        {
            return new Coordinate(a.X + b.X, a.Y + b.Y);
        }

        public static Coordinate operator -(Coordinate a, Coordinate b)
        {
            return new Coordinate(a.X - b.X, a.Y - b.Y);
        }

        public static implicit operator Vector2(Coordinate c)
        {
            return new Vector2(c.X, c.Y);
        }

        public static implicit operator Coordinate(Vector2 v)
        {
            return new Coordinate((int)Math.Round(v.X), (int)Math.Round(v.Y));
        }

        public override string ToString ()
        {
            return string.Format ("[Coordinate: X={0}, Y={1}]", X, Y);
        }

        public static Coordinate NorthWest = new Coordinate(-1, -1);
        public static Coordinate West = new Coordinate(0, -1);
        public static Coordinate SouthWest = new Coordinate(1, -1);
        public static Coordinate South = new Coordinate(1, 0);
        public static Coordinate SouthEast = new Coordinate(1, 1);
        public static Coordinate East = new Coordinate(0, 1);
        public static Coordinate NorthEast = new Coordinate(-1, 1);
        public static Coordinate North = new Coordinate(-1, 0);
    }
}
