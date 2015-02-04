using System.Collections.Generic;
using Engine;
using Engine.AI.Senses;
using Engine.Drawing;
using Engine.Entities;
using Engine.Maps;
using Engine.Sprites;
using Microsoft.Xna.Framework;
using ZombieUnknown.AI;

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
        
        public Human(string name, Sprite sprite, Coordinate mapPosition)
            : base(name, sprite, mapPosition)
        {
            //Vision = new Vision(VisionRange, FieldOfView);
            Mind = new HumanMind(this);
            IsStatic = false;
            //Vision.UpdateVisibility(mapPosition, FacingDirection);
        }

        public override void Update(GameTime gameTime)
        {
            Mind.Think();

            base.Update(gameTime);
        }
    }
}
