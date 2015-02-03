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
        public HumanMind(Human entity) : base(entity)
        {
            entity.SetAnimation("idle", GameState.GameTime);
        }

        public override void Think()
        {
            var seenSomething = false;
            //foreach (var entity in Entity.Vision.GetSeenEntities(Entity.GetCoordinate()))
            //{
            //    if (entity == Entity) continue;
            //    if (entity is DrawableEntity)
            //    {
            //        seenSomething = true;
            //        Console.WriteLine(DateTime.Now.ToLongTimeString() + " " + entity);
            //    }
            //}
            //if (!seenSomething) Console.WriteLine(DateTime.Now.ToLongTimeString() + " " + "Nothing Seen");




            if (Goals.Count == 0)
            {
                var stand = GameState.RandomNumberGenerator.Next(5) == 1;
                if (stand)
                {
                    var idleActivity = GameState.RandomNumberGenerator.Next(3);
                    switch (idleActivity)
                    {
                        case 0:
                            Goals.Push(new WaitGoal(Entity, GameState.RandomNumberGenerator.Next(2000)));
                            break;
                        case 1:
                            Goals.Push(new WaitGoal(Entity, GameState.RandomNumberGenerator.Next(2000)));
                            Goals.Push(new TurnGoal(Entity, TurnDirection.Right));
                            break;
                        case 2:
                            Goals.Push(new WaitGoal(Entity, GameState.RandomNumberGenerator.Next(2000)));
                            Goals.Push(new TurnGoal(Entity, TurnDirection.Left));
                            break;
                    }
                }
                else
                {
                    var newLocationX = GameState.RandomNumberGenerator.Next(GameState.Map.Width);
                    var newLocationY = GameState.RandomNumberGenerator.Next(GameState.Map.Height);
                    var run = GameState.RandomNumberGenerator.Next(20) == 1;

                    Goals.Push(new FollowPathGoal(Entity, new Coordinate(newLocationX, newLocationY), run));
                }
            }

            base.Think();
        }
    }
}
