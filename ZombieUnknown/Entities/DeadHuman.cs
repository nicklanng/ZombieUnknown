using Engine;
using Engine.Entities;
using ZombieUnknown.Entities.Mobiles;
using ZombieUnknown.InventoryObjects.Wearables;

namespace ZombieUnknown.Entities
{
    class DeadHuman : VisibleEntity
    {
        public WearableRig Rig { get; private set; }

        public DeadHuman(Human human)
            : base("dead" + human.Name, ResourceManager.GetSprite("deadHuman"), human.MapPosition)
        {
            Rig = human.Rig;
        }
    }
}