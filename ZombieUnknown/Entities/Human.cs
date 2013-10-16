using Engine;
using Engine.Entities;
using Engine.Sprites;

namespace ZombieUnknown.Entities
{
    class Human : MoveableEntity
    {
        public Human(string name, Sprite sprite, ISpriteDrawer spriteDrawer) 
            : base(name, sprite, spriteDrawer)
        {
        }
    }
}
