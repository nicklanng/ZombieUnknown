using Engine;
using Engine.Entities;
using Engine.Maps;
using Engine.Pathfinding;
using Microsoft.Xna.Framework;

namespace ZombieUnknown.Entities
{
    public class Lamp : VisibleEntity, ILightSource, IMovementBlocker
    {
        public Light Light { get; private set; }

        public Lamp(string name, Coordinate mapPosition) 
            : base(name, ResourceManager.GetSprite("lamp"), mapPosition)
        {
            Light = new Light(mapPosition, Color.White, 4);
        }

        public bool BlocksTile { get { return true; } }
        public bool BlocksDiagonals { get { return false; } }

    }
}
