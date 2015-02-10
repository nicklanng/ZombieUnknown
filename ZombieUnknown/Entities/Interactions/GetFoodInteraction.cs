using Engine.Entities.Interactions;
using ZombieUnknown.Entities.Mobiles;
using ZombieUnknown.InventoryObjects;

namespace ZombieUnknown.Entities.Interactions
{
    public class GetFoodInteraction : IInteraction<FoodContainer, Human>
    {
        public static string Text = "Get Food";

        public int MillisToCompleteAction
        {
            get { return 2000; }
        }

        public void Interact(FoodContainer subject, Human actor)
        {
            if (IsPossible(subject, actor))
            {
                var item = subject.Storage.TakeItemOfType<FoodObject>();
                actor.Hunger = 60;
                actor.GiveItem(item);
            }
        }

        public bool IsPossible(FoodContainer subject, Human actor)
        {
            return subject.Storage.HasItemOfType<FoodObject>();
        }
    }
}