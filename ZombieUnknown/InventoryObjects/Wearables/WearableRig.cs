using System.Collections.Generic;
using System.Linq;
using Engine.InventoryObjects;

namespace ZombieUnknown.InventoryObjects.Wearables
{
    class WearableRig
    {
        private readonly Dictionary<WearableType, IWearable> _wearables;

        public WearableRig()
        {
            _wearables = new Dictionary<WearableType, IWearable>();
        }

        public IWearable PutOn(IWearable wearable)
        {
            IWearable result = null;

            if (_wearables.ContainsKey(wearable.WearableType))
            {
                result = _wearables[wearable.WearableType];
            }
            _wearables[wearable.WearableType] = wearable;

            return result;
        }

        public StorageCollection GetInventories()
        {
            return _wearables.Values.OfType<IStorage>().Select(x => x.Storage).ToStorageCollection();
        }
    }
}
