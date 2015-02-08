using Engine.InventoryObjects;

namespace ZombieUnknown.InventoryObjects.Wearables
{
    class Backback : IStorage, IWearable
    {
        public WearableType WearableType { get { return WearableType.Backpack; } }
        public Storage Storage { get; private set; }

        public Backback()
        {
            Storage = new Storage(new Size(5, 6));
        }
    }
}
