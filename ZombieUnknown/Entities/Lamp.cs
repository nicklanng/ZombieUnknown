using Engine;
using Engine.Entities;
using Engine.Maps;
using Engine.Pathfinding;
using Microsoft.Xna.Framework;

namespace ZombieUnknown.Entities
{
    public class Lamp : PhysicalEntity, ILightSource, IMovementBlocker
    {
        public Light Light { get; private set; }

        public Lamp(string name, Coordinate mapPosition) 
            : base(name, ResourceManager.GetSprite("lamp"), mapPosition)
        {
            Light = new Light(mapPosition, Color.White, 4);
        }

        public override float Speed
        {
            get { return 0; }
        }

        public bool BlocksTile { get { return true; } }
        public bool BlocksDiagonals { get { return false; } }

    }
}
