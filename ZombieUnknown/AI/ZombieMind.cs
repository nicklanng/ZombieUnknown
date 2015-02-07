using System;
using Engine.AI;
using ZombieUnknown.Entities;

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
