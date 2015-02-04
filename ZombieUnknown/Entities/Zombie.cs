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
    class Zombie : PhysicalEntity
    {
        private const int VisionRange = 10;
        private const int FieldOfView = 90;

        public override float Speed
        {
            get { return 5; }
        }

        public ZombieMind Mind { get; private set; }
        public Vision Vision { get; private set; }

        public Zombie(string name, Sprite sprite, Coordinate coordinate)
            : base(name, sprite, coordinate)
        {
            Vision = new Vision(VisionRange, FieldOfView);
            Mind = new ZombieMind(this);
            IsStatic = false;
            Vision.UpdateVisibility(coordinate, FacingDirection);
        }

        public override void SetCoordinate(Coordinate coordinate)
        {
            base.SetCoordinate(coordinate);
            Vision.UpdateVisibility(GetCoordinate(), FacingDirection);
        }

        public override void FaceDirection(IDirection direction)
        {
            base.FaceDirection(direction);
            Vision.UpdateVisibility(GetCoordinate(), FacingDirection);
        }

        public override void Update(GameTime gameTime)
        {
            Mind.Think();

            base.Update(gameTime);
        }

        public override IEnumerable<DrawingRequest> GetDrawings()
        {
            yield return new DrawingRequest(Sprite, MapPosition, GameState.Map.GetTile(GetCoordinate()).Light);
        }
    }
}
