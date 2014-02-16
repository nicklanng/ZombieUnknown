using System.Linq;
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
            if (!Goals.Any())
            {
                Goals.Push(new FollowPathGoal(Entity, new Coordinate(0, 0)));
                Goals.Push(new FollowPathGoal(Entity, new Coordinate(0, 2)));
            }

            base.Think();
        }
    }
}
