using System;
using Engine.AI;
using ZombieUnknown.Entities;

namespace ZombieUnknown.AI
{
    class ZombieMind : Mind<Zombie>
    {
        protected Random RandomNumberGenerator;

        public ZombieMind(Zombie entity) : base(entity)
        {
            RandomNumberGenerator = new Random();
        }

        public override void Think()
        {
            // find alive food
            foreach (var seenEntity in Entity.Vision.GetSeenEntities(Entity.GetCoordinate()))
            {
                if (seenEntity is Human)
                {
                    //Goals.Push(new FollowPathGoal(Entity, seenEntity.GetCoordinate()));
                }
            }

            // wander
            if (Goals.Count == 0)
            {
                Goals.Push(new WaitGoal(Entity, 1000));

                var result = RandomNumberGenerator.Next(2);
                if (result == 0) 
                {
                    Goals.Push (new TurnGoal (Entity, TurnDirection.Right));
                } 
                else 
                {
                    Goals.Push (new TurnGoal (Entity, TurnDirection.Left));
                }
            }

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
