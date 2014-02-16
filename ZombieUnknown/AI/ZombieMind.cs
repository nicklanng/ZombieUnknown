using Engine.AI;
using Engine.Maps;
using ZombieUnknown.Entities;

namespace ZombieUnknown.AI
{
    class ZombieMind : Mind<Zombie>
    {
        public ZombieMind(Zombie entity) : base(entity)
        {
        }

        public override void Think()
        {
            // find alive food
                // go to alive food
                    // kill alive food
            // find fresh food
                // go to fresh food
                    // eat fresh food
            // wander

            base.Think();
        }
    }
}
