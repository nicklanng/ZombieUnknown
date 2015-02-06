using System.Collections.Generic;
using Engine;
using Engine.Entities;
using Engine.Entities.Interactions;
using Engine.Maps;
using ZombieUnknown.Entities.Interactions;

namespace ZombieUnknown.Entities
{
    public class CultivatedLand : PhysicalEntity, IInteractable
    {
        public CultivatedLand(string name, Coordinate mapPosition) 
            : base(name, ResourceManager.GetSprite("cultivatedLand"), mapPosition)
        {
        }

        public override float Speed
        {
            get { return 0; }
        }

        public AccessPosition[] AccessPositions
        {
            get {
                return new []
                {
                    new AccessPosition (Direction.South.Coordinate, Direction.North),
                    new AccessPosition (Direction.North.Coordinate, Direction.South),
                    new AccessPosition (Direction.West.Coordinate, Direction.East),
                    new AccessPosition (Direction.East.Coordinate, Direction.West),
                    new AccessPosition (Direction.NorthEast.Coordinate, Direction.SouthWest),
                    new AccessPosition (Direction.SouthEast.Coordinate, Direction.NorthWest),
                    new AccessPosition (Direction.NorthWest.Coordinate, Direction.SouthEast),
                    new AccessPosition (Direction.SouthWest.Coordinate, Direction.NorthEast),
                };
            }
        }

        public Dictionary<string, IInteraction> Interactions
        {
            get
            {
                return new Dictionary<string, IInteraction>
                {
                    { SowSeedInteraction.Text, new SowSeedInteraction(this) }
                };
            }
        }
    }
}
