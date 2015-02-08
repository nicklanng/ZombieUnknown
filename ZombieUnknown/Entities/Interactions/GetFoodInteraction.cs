using Engine.Entities;
using Engine.Entities.Interactions;
using ZombieUnknown.Entities.Mobiles;
using ZombieUnknown.InventoryObjects;

namespace ZombieUnknown.Entities.Interactions
{
    public class GetFoodInteraction : Interaction
    {
        public static string Text = "Get Food";

        public override int MillisToCompleteAction { get { return 2000; } }

        public GetFoodInteraction(PhysicalEntity subject) 
            : base(subject)
        {
        }

        public override void Interact(PhysicalEntity actor)
        {
            var human = (Human)actor;
            if (human != null)
            {
                human.Hunger = 60;
                human.GiveItem(new FoodObject());
            }

        }
    }
}
