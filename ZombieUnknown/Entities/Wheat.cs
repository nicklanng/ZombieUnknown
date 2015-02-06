using System;
using Engine.Entities;
using Engine.Maps;
using Engine;

namespace ZombieUnknown.Entities
{
    public class Wheat : PhysicalEntity
    {
        public Wheat(string name, Coordinate mapPosition)
            : base(name, ResourceManager.GetSprite("wheat"), mapPosition)
        {
        }

        public override float Speed {
            get {
                return 0;
            }
        }
    }
}

