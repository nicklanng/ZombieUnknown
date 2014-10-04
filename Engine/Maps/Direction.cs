using System;
using System.Collections.Generic;

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
    }
        
    public class North : IDirection
    {
        public Coordinate Coordinate { get { return Coordinate.UpRight; } }
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
        public Coordinate Coordinate { get { return Coordinate.Right; } }
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
        public Coordinate Coordinate { get { return Coordinate.DownRight; } }
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
        public Coordinate Coordinate { get { return Coordinate.Down; } }
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
        public Coordinate Coordinate { get { return Coordinate.DownLeft; } }
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
        public Coordinate Coordinate { get { return Coordinate.Left; } }
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
        public Coordinate Coordinate { get { return Coordinate.UpLeft; } }
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
        public Coordinate Coordinate { get { return Coordinate.Up; } }
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