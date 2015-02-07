using System;
using System.Collections.Generic;
using Engine;
using Engine.AI.Senses;
using Engine.Drawing;
using Engine.Entities;
using Engine.Maps;
using Microsoft.Xna.Framework;
using ZombieUnknown.AI;
using Console = Engine.Drawing.Console;

namespace ZombieUnknown.Entities
{
    public class Human : MobileEntity, IMainCharacter
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

        public Human(string name, Coordinate mapPosition)
            : base(name, ResourceManager.GetSprite("human"), mapPosition)
        {
            Mind = new HumanMind(this);
            IsStatic = false;

            Hunger = 21;
        }

        public override void Update()
        {
            Mind.Think();

            Hunger -= GameState.GameTime.ElapsedGameTime.TotalSeconds;
            Console.WriteLine("Hunger: " + Math.Ceiling(Hunger));

            base.Update();
        }

        public override IEnumerable<DrawingRequest> GetDrawings()
        {
            yield return new DrawingRequest(Sprite, MapPosition, LightValue, new Vector2(0.000001f, 0.000001f));
        }
    }
}
