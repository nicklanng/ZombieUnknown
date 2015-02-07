using Engine.AI.Senses;
using Engine.Entities;
using Engine.Maps;
using Engine.Sprites;
using Microsoft.Xna.Framework;
using ZombieUnknown.AI;

namespace ZombieUnknown.Entities
{
    class Zombie : MobileEntity
    {
        private const int VisionRange = 10;
        private const int FieldOfView = 90;

        public override float Speed
        {
            get { return 5; }
        }

        public ZombieMind Mind { get; private set; }
        public Vision Vision { get; private set; }

        public Zombie(string name, Sprite sprite, Coordinate mapPosition)
            : base(name, sprite, mapPosition)
        {
            Vision = new Vision(VisionRange, FieldOfView);
            Mind = new ZombieMind(this);
            IsStatic = false;
        }

        public override void Update(GameTime gameTime)
        {
            Mind.Think();

            base.Update(gameTime);
        }
    }
}
