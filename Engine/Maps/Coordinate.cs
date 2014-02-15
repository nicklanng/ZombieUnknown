﻿using System;
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

        public static Coordinate FromVector2(Vector2 mapPosition)
        {
            return new Coordinate((int)mapPosition.X, (int)mapPosition.Y);
        }

        public Vector2 ToVector2()
        {
            return new Vector2(X, Y);
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

        public static Coordinate Up = new Coordinate(-1, -1);
        public static Coordinate UpLeft = new Coordinate(0, -1);
        public static Coordinate Left = new Coordinate(1, -1);
        public static Coordinate DownLeft = new Coordinate(1, 0);
        public static Coordinate Down = new Coordinate(1, 1);
        public static Coordinate DownRight = new Coordinate(0, 1);
        public static Coordinate Right = new Coordinate(-1, 1);
        public static Coordinate UpRight = new Coordinate(-1, 0);
    }
}
