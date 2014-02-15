using System.Collections.Generic;
using Engine.AI;
using Engine.Drawing;
using Engine.Entities;
using Engine.Maps;
using Engine.Sprites;
using Microsoft.Xna.Framework;

namespace ZombieUnknown.Entities
{
    public class Human : MoveableEntity
    {
        public Mind<Human> Mind { get; private set; } 

        public Human(string name, Sprite sprite, Coordinate coordinate)
            : base(name, sprite, coordinate)
        {
            Mind = new Mind<Human>(this);
            MapPosition = Coordinate.ToVector2();
        }
        
        public override void Update(GameTime gameTime)
        {
            Mind.Think();

            base.Update(gameTime);
        }

        public override IEnumerable<DrawingRequest> GetDrawings()
        {
            yield return new DrawingRequest(Sprite, MapPosition, Color.White);
        }
    }
}
