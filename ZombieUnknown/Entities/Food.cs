using Engine.Entities;
using Engine.Entities.Interactions;
using Engine.Maps;
using Engine.Pathfinding;
using Engine.Sprites;
using Microsoft.Xna.Framework;
using ZombieUnknown.Entities.Interactions;

namespace ZombieUnknown.Entities
{
    class Food : PhysicalEntity, IMovementBlocker, IInteractable
    {
        public Food(string name, Sprite sprite, Vector2 mapPosition) 
            : base(name, sprite, mapPosition)
        {
        }

        public override float Speed
        {
            get { return 0; }
        }

        public bool BlocksTile { get { return true; } }
        public bool BlocksDiagonals { get { return true; } }

        public AccessPosition[] AccessPositions {
            get {
                return new [] {new AccessPosition (Direction.South.Coordinate, Direction.North)};
            }
        }

        public IInteraction[] Interactions
        {
            get { return new IInteraction[] { new GetFoodInteraction() }; }
        }
    }
}
