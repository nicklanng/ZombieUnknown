using Engine.Entities;
using Engine.Maps;
using Engine.Pathfinding;
using Engine.Sprites;
using Microsoft.Xna.Framework;

namespace ZombieUnknown.Entities
{
    class Food : PhysicalEntity, IMovementBlocker
    {
        public Food(string name, Sprite sprite, Vector2 mapPosition) 
            : base(name, sprite, mapPosition)
        {
        }

        public override float Speed
        {
            get { return 0; }
        }

        public bool BlocksTile { get { return true; } }
        public bool BlocksDiagonals { get { return true; } }
    }
}
