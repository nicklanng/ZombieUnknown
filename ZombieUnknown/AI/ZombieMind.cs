using System;
using Engine.AI;
using ZombieUnknown.Entities;
using ZombieUnknown.Entities.Mobiles;

namespace ZombieUnknown.AI
{
    class ZombieMind
    {
        protected Random RandomNumberGenerator;

        public ZombieMind(Zombie entity)
        {
            RandomNumberGenerator = new Random();
        }

        public void Think()
        {
        }
    }
}
