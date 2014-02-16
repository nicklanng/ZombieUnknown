using System.Collections.Generic;
using Engine;
using Engine.Drawing;
using Engine.Entities;
using Engine.Maps;
using Engine.Sprites;
using Microsoft.Xna.Framework;
using ZombieUnknown.AI;

namespace ZombieUnknown.Entities
{
    public class Human : DrawableEntity
    {
        public override float Speed
        {
            get { return 10; }
        }

        public HumanMind Mind { get; private set; } 

        public Human(string name, Sprite sprite, Coordinate coordinate)
            : base(name, sprite, coordinate)
        {
            Mind = new HumanMind(this);
        }

        public override void Update(GameTime gameTime)
        {
            Mind.Think();

            base.Update(gameTime);
        }

        public override IEnumerable<DrawingRequest> GetDrawings()
        {
            yield return new DrawingRequest(Sprite, MapPosition, GameState.Map.GetTile(Coordinate).Light);
        }
    }
}
