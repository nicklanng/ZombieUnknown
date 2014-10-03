using System;
using Engine;
using Engine.AI;
using Engine.Maps;
using ZombieUnknown.Entities;

namespace ZombieUnknown.AI
{
    public class HumanMind : Mind<Human>
    {
        protected Random _randomNumberGenerator;

        public HumanMind(Human entity) : base(entity)
        {
            _randomNumberGenerator = new Random();
        }

        public override void Think()
        {
            if (Goals.Count == 0)
            {
                var newLocationX = _randomNumberGenerator.Next(GameState.Map.Width);
                var newLocationY = _randomNumberGenerator.Next(GameState.Map.Height);

                Goals.Push (new FollowPathGoal(Entity, new Coordinate(newLocationX, newLocationY)));
            }

            base.Think();
        }
    }
}
