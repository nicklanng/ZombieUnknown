namespace Engine.InventoryObjects
{
    public class Storage
    {
        private readonly bool[,] _storeMask;

        public Size Space { get; private set; }
        public IInventoryObject[,] Store { get; private set; }
        
        public Storage(Size size)
        {
            Space = size;
            Store = new IInventoryObject[Space.Width, Space.Height];
            _storeMask = new bool[Space.Width, Space.Height];
        }

        public StorageLocation GetAvailableSlot(IInventoryObject inventoryObject)
        {
            var itemSize = inventoryObject.Size;

            if (itemSize > Space) return null;

            for (short x = 0; x < Space.Width; x++)
            {
                for (short y = 0; y < Space.Height; y++)
                {
                    if (_storeMask[x, y]) continue;

                    if (x + itemSize.Width > Space.Width) continue;
                    if (y + itemSize.Height > Space.Height) continue;

                    for (short itemX = 0; itemX < itemSize.Width; itemX++)
                    {
                        for (short itemY = 0; itemY < itemSize.Height; itemY++)
                        {
                            if (_storeMask[x + itemX, y + itemY]) continue;
                            return new StorageLocation(x, y);
                        }
                    }
                }
            }

            return null;
        }

        public void Insert(StorageLocation location, IInventoryObject inventoryObject)
        {
            Store[location.X, location.Y] = inventoryObject;
            RebuildStoreMask();
        }

        private void RebuildStoreMask()
        {
            _storeMask.Initialize();

            for (short x = 0; x < Space.Width; x++)
            {
                for (short y = 0; y < Space.Height; y++)
                {
                    var item = Store[x, y];
                    if (item == null) continue;

                    var itemSize = item.Size;
                    for (short itemX = 0; itemX < itemSize.Width; itemX++)
                    {
                        for (short itemY = 0; itemY < itemSize.Height; itemY++)
                        {
                            _storeMask[x + itemX, y + itemY] = true;
                        }
                    }
                }
            }
        }
    }
}
