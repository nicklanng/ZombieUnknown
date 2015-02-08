using System.Collections.Generic;
using Engine;
using Engine.Entities;
using Engine.Entities.Interactions;
using Engine.Maps;
using Engine.Pathfinding;
using ZombieUnknown.Entities.Interactions;

namespace ZombieUnknown.Entities
{
    class Food : PhysicalEntity, IMovementBlocker
    {
        public Food(string name, Coordinate mapPosition) 
            : base(name, ResourceManager.GetSprite("food"), mapPosition)
        {
        }

        public bool BlocksTile { get { return true; } }
        public bool BlocksDiagonals { get { return true; } }

        public override AccessPosition[] AccessPositions
        {
            get {
                return new [] {new AccessPosition (Direction.South.Coordinate, Direction.North)};
            }
        }

        protected override Dictionary<string, IInteraction> InteractionList
        {
            get { 
                return new Dictionary<string, IInteraction>
                {
                    { GetFoodInteraction.Text, new GetFoodInteraction() }
                }; 
            }
        }
    }
}
