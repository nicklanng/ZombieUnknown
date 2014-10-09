using System.Collections.Generic;
using Engine;
using Engine.Drawing;
using Engine.Entities;
using Engine.Maps;
using Engine.Sprites;

namespace ZombieUnknown.Entities
{
    class TallGrass1 : DrawableEntity
    {
        public TallGrass1(string name, Sprite sprite, Coordinate coordinate) 
            : base(name, sprite, coordinate)
        {
        }

        public override float Speed
        {
            get { return 0; }
        }

        public override IEnumerable<DrawingRequest> GetDrawings()
        {
            yield return new DrawingRequest(Sprite, MapPosition, GameState.Map.GetTile(GetCoordinate()).Light);
        }
    }
}
