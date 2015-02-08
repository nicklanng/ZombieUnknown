namespace Engine.InventoryObjects
{
    public class StorageLocation
    {
        public short X { get; private set; }
        public short Y { get; private set; }

        public StorageLocation(short x, short y)
        {
            X = x;
            Y = y;
        }
    }
}
