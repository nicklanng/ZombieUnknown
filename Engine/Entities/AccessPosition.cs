using System;
using Microsoft.Xna.Framework;
using Engine.Maps;

namespace Engine.Entities
{
    public class AccessPosition
    {
        public Vector2 PositionOffset { get; private set; }
        public IDirection Direction { get; private set; }

        public AccessPosition (Vector2 positionOffset, IDirection direction)
        {
            PositionOffset = positionOffset;
            Direction = direction;
        }
    }
}

