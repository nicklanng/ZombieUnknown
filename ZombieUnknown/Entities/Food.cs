using Engine.Entities;
using Engine.Sprites;
using Microsoft.Xna.Framework;

namespace ZombieUnknown.Entities
{
    class Food : PhysicalEntity
    {
        public Food(string name, Sprite sprite, Vector2 mapPosition) 
            : base(name, sprite, mapPosition)
        {
            BlocksTiles = true;
        }

        public override float Speed
        {
            get { return 0; }
        }
    }
}
