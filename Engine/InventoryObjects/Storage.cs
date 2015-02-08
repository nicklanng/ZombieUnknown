using System;
using System.Collections.Generic;

namespace Engine.InventoryObjects
{
    public class Storage
    {
        private readonly bool[,] _storeMask;

        private readonly Size _space;
        private readonly IInventoryObject[,] _store;
        
        public Storage(Size size)
        {
            _space = size;
            _store = new IInventoryObject[_space.Width, _space.Height];
            _storeMask = new bool[_space.Width, _space.Height];
        }

        public StorageLocation GetAvailableSlot(IInventoryObject inventoryObject)
        {
            var itemSize = inventoryObject.Size;

            if (itemSize > _space) return null;

            for (short x = 0; x < _space.Width; x++)
            {
                for (short y = 0; y < _space.Height; y++)
                {
                    if (_storeMask[x, y]) continue;

                    if (x + itemSize.Width > _space.Width) continue;
                    if (y + itemSize.Height > _space.Height) continue;

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
            _store[location.X, location.Y] = inventoryObject;
            RebuildStoreMask();
        }

        public IEnumerable<Tuple<StorageLocation, IInventoryObject>> ListItems()
        {
            for (short x = 0; x < _space.Width; x++)
            {
                for (short y = 0; y < _space.Height; y++)
                {
                    var item = _store[x, y];
                    if (item != null) yield return new Tuple<StorageLocation, IInventoryObject>(new StorageLocation(x, y), item);
                }
            }
        }

        public IInventoryObject TakeItemAt(StorageLocation locationOfItemToGet)
        {
            var item = _store[locationOfItemToGet.X, locationOfItemToGet.Y];
            _store[locationOfItemToGet.X, locationOfItemToGet.Y] = null;

            RebuildStoreMask();

            return item;
        }

        private void RebuildStoreMask()
        {
            _storeMask.Initialize();

            for (short x = 0; x < _space.Width; x++)
            {
                for (short y = 0; y < _space.Height; y++)
                {
                    var item = _store[x, y];
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
