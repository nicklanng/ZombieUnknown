using System.Collections.Generic;
using Engine.Drawing;
using Engine.Entities;
using Engine.Sprites;
using Microsoft.Xna.Framework;

namespace ZombieUnknown.Entities
{
    public class CultivatedLand : PhysicalEntity
    {
        public CultivatedLand(string name, Sprite sprite, Vector2 mapPosition) 
            : base(name, sprite, mapPosition)
        {
        }

        public override float Speed
        {
            get { return 0; }
        }
    }
}
