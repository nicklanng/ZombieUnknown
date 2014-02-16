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
    class Zombie : DrawableEntity
    {
        public ZombieMind Mind { get; private set; } 

        public Zombie(string name, Sprite sprite, Coordinate coordinate)
            : base(name, sprite, coordinate)
        {
            Mind = new ZombieMind(this);
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
