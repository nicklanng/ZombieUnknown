using Engine.Entities;
using Engine.Maps;
using Engine.Sprites;

namespace ZombieUnknown.Entities
{
    class TallGrass1 : PhysicalEntity
    {
        public TallGrass1(string name, Sprite sprite, Coordinate mapPosition) 
            : base(name, sprite, mapPosition)
        {
        }

        public override float Speed
        {
            get { return 0; }
        }
    }
}
