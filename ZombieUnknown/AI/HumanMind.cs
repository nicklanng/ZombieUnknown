using System;
using Engine;
using Engine.AI;
using Engine.Entities;
using Engine.Maps;
using ZombieUnknown.Entities;

using Console = Engine.Drawing.Console;

namespace ZombieUnknown.AI
{
    public class HumanMind : Mind<Human>
    {
        protected Random RandomNumberGenerator;

        public HumanMind(Human entity) : base(entity)
        {
            RandomNumberGenerator = new Random();
        }

        public override void Think()
        {
            var seenSomething = false;
            foreach (var entity in Entity.Vision.GetSeenEntities(Entity.GetCoordinate()))
            {
                if (entity == Entity) continue;
                if (entity is DrawableEntity)
                {
                    seenSomething = true;
                    Console.WriteLine(DateTime.Now.ToLongTimeString() + " " + entity);
                }
            }
            if (!seenSomething) Console.WriteLine(DateTime.Now.ToLongTimeString() + " " + "Nothing Seen");




            if (Goals.Count == 0)
            {
                var newLocationX = RandomNumberGenerator.Next(GameState.Map.Width);
                var newLocationY = RandomNumberGenerator.Next(GameState.Map.Height);

                Goals.Push(new FollowPathGoal(Entity, new Coordinate(newLocationX, newLocationY)));
            }

            base.Think();
        }
    }
}
