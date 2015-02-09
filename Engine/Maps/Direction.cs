using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Engine.Maps
{
    public interface IDirection
    {
        Coordinate Coordinate { get; }
        IDirection TurnLeft();
        IDirection TurnRight();
    }

    public static class Direction
    {
        public static IDirection North = new North();
        public static IDirection NorthEast = new NorthEast();
        public static IDirection East = new East();
        public static IDirection SouthEast = new SouthEast();
        public static IDirection South = new South();
        public static IDirection SouthWest = new SouthWest();
        public static IDirection West = new West();
        public static IDirection NorthWest = new NorthWest();

        public static Dictionary<Coordinate, IDirection> CoordinateDirectionMap = new Dictionary<Coordinate, IDirection> 
        {
            {North.Coordinate, North},
            {NorthEast.Coordinate, NorthEast},
            {East.Coordinate, East},
            {SouthEast.Coordinate, SouthEast},
            {South.Coordinate, South},
            {SouthWest.Coordinate, SouthWest},
            {West.Coordinate, West},
            {NorthWest.Coordinate, NorthWest}
        };

        public static IDirection GetDirectionFromVector(Vector2 directionVector)
        {
            var angleOfTravel = (float)(Math.Atan2(North.Coordinate.Y, North.Coordinate.X) - Math.Atan2(directionVector.Y, directionVector.X));
            var degrees = ((MathHelper.ToDegrees(angleOfTravel) + 360 + 22.5) % 360);
            var direction = (int)Math.Floor(degrees / 45);

            switch (direction)
            {
                case 0:
                    return North;
                case 1:
                    return NorthEast;
                case 2:
                    return East;
                case 3:
                    return SouthEast;
                case 4:
                    return South;
                case 5:
                    return SouthWest;
                case 6:
                    return West;
                case 7:
                    return NorthWest;
            }

            return North;
        }
    }
        
    public class North : IDirection
    {
        public Coordinate Coordinate { get { return Coordinate.North; } }
        public IDirection TurnLeft()
        {
            return Direction.NorthWest;
        }
        public IDirection TurnRight()
        {
            return Direction.NorthEast;
        }

        public override string ToString ()
        {
            return this.GetType().Name;
        }
    }

    public class NorthEast : IDirection
    {
        public Coordinate Coordinate { get { return Coordinate.NorthEast; } }
        public IDirection TurnLeft()
        {
            return Direction.North;
        }
        public IDirection TurnRight()
        {
            return Direction.East;
        }

        public override string ToString ()
        {
            return this.GetType().Name;
        }
    }

    public class East : IDirection
    {
        public Coordinate Coordinate { get { return Coordinate.East; } }
        public IDirection TurnLeft()
        {
            return Direction.NorthEast;
        }
        public IDirection TurnRight()
        {
            return Direction.SouthEast;
        }

        public override string ToString ()
        {
            return this.GetType().Name;
        }
    }

    public class SouthEast : IDirection
    {
        public Coordinate Coordinate { get { return Coordinate.SouthEast; } }
        public IDirection TurnLeft()
        {
            return Direction.East;
        }
        public IDirection TurnRight()
        {
            return Direction.South;
        }

        public override string ToString ()
        {
            return this.GetType().Name;
        }
    }

    public class South : IDirection
    {
        public Coordinate Coordinate { get { return Coordinate.South; } }
        public IDirection TurnLeft()
        {
            return Direction.SouthEast;
        }
        public IDirection TurnRight()
        {
            return Direction.SouthWest;
        }

        public override string ToString ()
        {
            return this.GetType().Name;
        }
    }

    public class SouthWest : IDirection
    {
        public Coordinate Coordinate { get { return Coordinate.SouthWest; } }
        public IDirection TurnLeft()
        {
            return Direction.South;
        }
        public IDirection TurnRight()
        {
            return Direction.West;
        }

        public override string ToString ()
        {
            return this.GetType().Name;
        }
    }

    public class West : IDirection
    {
        public Coordinate Coordinate { get { return Coordinate.West; } }
        public IDirection TurnLeft()
        {
            return Direction.SouthWest;
        }
        public IDirection TurnRight()
        {
            return Direction.NorthWest;
        }

        public override string ToString ()
        {
            return this.GetType().Name;
        }
    }
    

    public class NorthWest : IDirection
    {
        public Coordinate Coordinate { get { return Coordinate.NorthWest; } }
        public IDirection TurnLeft()
        {
            return Direction.West;
        }
        public IDirection TurnRight()
        {
            return Direction.North;
        }

        public override string ToString ()
        {
            return this.GetType().Name;
        }
    }
}