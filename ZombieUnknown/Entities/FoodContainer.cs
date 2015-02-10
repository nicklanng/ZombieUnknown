using Engine;
using Engine.Entities;
using Engine.InventoryObjects;
using Engine.Maps;
using Engine.Pathfinding;
using ZombieUnknown.InventoryObjects;

namespace ZombieUnknown.Entities
{
    public class FoodContainer : VisibleEntity, IMovementBlocker, IStorage
    {
        public Storage Storage { get; private set; }

        public bool BlocksTile
        {
            get { return true; }
        }

        public bool BlocksDiagonals
        {
            get { return true; }
        }

        public FoodContainer(string name, Coordinate mapPosition)
            : base(name, ResourceManager.GetSprite("food"), mapPosition)
        {
            Storage = new Storage(new Size(7, 9));
            Storage.Insert(new StorageLocation(1, 5), new FoodObject());
        }

        public override AccessPosition[] AccessPositions
        {
            get { return new[] { new AccessPosition(Direction.South.Coordinate, Direction.North) }; }
        }
    }
}