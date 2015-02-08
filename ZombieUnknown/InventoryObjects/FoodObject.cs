using Engine.InventoryObjects;

namespace ZombieUnknown.InventoryObjects
{
    class FoodObject : IInventoryObject
    {
        public Size Size { get { return new Size(1, 1); } }
    }
}
