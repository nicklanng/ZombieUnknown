using Engine.Entities;
using Engine.Entities.Interactions;
using Engine.InventoryObjects;
using ZombieUnknown.Entities.Mobiles;
using ZombieUnknown.InventoryObjects;

namespace ZombieUnknown.Entities.Interactions
{
    public class GetFoodInteraction : Interaction
    {
        public static string Text = "Get Food";

        public override int MillisToCompleteAction { get { return 2000; } }
        
        public override void Interact(MobileEntity actor, PhysicalEntity subject)
        {
            var inventory = ((IStorage)subject).Storage;
            var items = inventory.ListItems();

            StorageLocation locationOfItemToGet = null;
            foreach (var tuple in items)
            {
                var storageLocation = tuple.Item1;
                var item = tuple.Item2;

                if (item is FoodObject)
                {
                    locationOfItemToGet = storageLocation;
                }
            }

            if (locationOfItemToGet != null)
            {
                var item = inventory.TakeItemAt(locationOfItemToGet);
                
                var human = (Human)actor;
                if (human != null)
                {
                    human.Hunger = 60;
                    human.GiveItem(item);
                }
            }

        }
    }
}
