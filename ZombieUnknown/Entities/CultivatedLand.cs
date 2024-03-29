using System.Collections.Generic;
using Engine;
using Engine.Entities;
using Engine.Entities.Interactions;
using Engine.Maps;
using ZombieUnknown.AI.Tasks;
using ZombieUnknown.Entities.Interactions;

namespace ZombieUnknown.Entities
{
    public class CultivatedLand : VisibleEntity
    {
        public CultivatedLand(string name, Coordinate mapPosition) 
            : base(name, ResourceManager.GetSprite("cultivatedLand"), mapPosition)
        {
            GameState.TaskList.AddTask(new SowWheatSeedTask(this));
        }

        public override AccessPosition[] AccessPositions
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

        protected override Dictionary<string, Interaction> InteractionList
        {
            get
            {
                return new Dictionary<string, Interaction>
                {
                    { SowSeedInteraction.Text, new SowSeedInteraction() }
                };
            }
        }
    }
}
