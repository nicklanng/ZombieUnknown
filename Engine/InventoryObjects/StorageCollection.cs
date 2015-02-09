using System.Collections.Generic;
using System.Linq;

namespace Engine.InventoryObjects
{
    public class StorageCollection : List<Storage>
    {
        public StorageCollection(IEnumerable<Storage> storage) : base(storage)
        {
        }

        public bool HasItemOfType<T>() where T : IInventoryObject
        {
            return this.Any(st => st.HasItemOfType<T>());
        }

        public T TakeItemOfType<T>() where T : IInventoryObject
        {
            var storageOfType = this.First(st => st.HasItemOfType<T>());
            return storageOfType.TakeItemOfType<T>();
        }
    }

    public static class StorageCollectionExtensions
    {
        public static StorageCollection ToStorageCollection(this IEnumerable<Storage> storage)
        {
            return new StorageCollection(storage);
        }
    }
}