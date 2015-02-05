using System;
using Engine.AI.Senses;
using Engine.Entities;
using Engine.Maps;
using Engine.Sprites;
using Microsoft.Xna.Framework;
using ZombieUnknown.AI;
using Console = Engine.Drawing.Console;

namespace ZombieUnknown.Entities
{
    public class Human : PhysicalEntity, IMainCharacter
    {
        private const int VisionRange = 10;
        private const int FieldOfView = 90;

        public override float Speed
        {
            get { return IsRunning ? 30 : 15; }
        }

        public HumanMind Mind { get; private set; }
        public Vision Vision { get; private set; }
        public double Hunger { get; set; }

        public Human(string name, Sprite sprite, Coordinate mapPosition)
            : base(name, sprite, mapPosition)
        {
            Mind = new HumanMind(this);
            IsStatic = false;

            Hunger = 70;
        }

        public override void Update(GameTime gameTime)
        {
            Mind.Think();

            Hunger -= gameTime.ElapsedGameTime.TotalSeconds;
            Console.WriteLine("Hunger: " + Math.Ceiling(Hunger));

            base.Update(gameTime);
        }
    }
}
