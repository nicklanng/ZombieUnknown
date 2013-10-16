using Engine.Entities;
using Engine.Sprites;

namespace ZombieUnknown.Entities
{
    interface IEntityFactory : IBaseEntityFactory
    {
        Human CreateHuman(string name, AnimatedSprite sprite);
    }
}
