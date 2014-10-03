using System;
using Engine.AI;
using Engine.Maps;
using ZombieUnknown.Entities;

namespace ZombieUnknown.AI
{
    class ZombieMind : Mind<Zombie>
    {
        protected Random _randomNumberGenerator;

        public ZombieMind(Zombie entity) : base(entity)
        {
            _randomNumberGenerator = new Random();
        }

        public override void Think()
        {
            if (Goals.Count == 0)
            {
                Goals.Push(new WaitGoal(1000));

                var result = _randomNumberGenerator.Next(2);
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
