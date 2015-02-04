using Engine.Entities;
using Engine.Pathfinding;
using Engine.Sprites;
using Microsoft.Xna.Framework;

namespace ZombieUnknown.Entities
{
    public class Lamp : PhysicalEntity, ILightSource, IMovementBlocker
    {
        public Light Light { get; private set; }

        public Lamp(string name, Sprite sprite, Vector2 mapPosition) : base(name, sprite, mapPosition)
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
