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
    public class Human : DrawableEntity, IMainCharacter
    {
        private const int VisionRange = 10;

        public override float Speed
        {
            get { return 10; }
        }

        public HumanMind Mind { get; private set; }
        public Vision Vision { get; private set; }
        
        public Human(string name, Sprite sprite, Coordinate coordinate)
            : base(name, sprite, coordinate)
        {
            Vision = new Vision(VisionRange);
            Mind = new HumanMind(this);
            IsStatic = false;
            Vision.UpdateVisibility(coordinate);
        }

        public override void SetCoordinate(Coordinate coordinate)
        {
            base.SetCoordinate(coordinate);
            Vision.UpdateVisibility(coordinate);
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
