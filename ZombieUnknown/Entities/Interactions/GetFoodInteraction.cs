using Engine.Entities.Interactions;
using ZombieUnknown.Entities.Mobiles;
using ZombieUnknown.InventoryObjects;

namespace ZombieUnknown.Entities.Interactions
{
    public class GetFoodInteraction : InteractionSingleton<FoodContainer, Human>
    {
        public static string Text = "Get Food";

        public override int MillisToCompleteAction
        {
            get { return 2000; }
        }

        protected override void Execute(FoodContainer subject, Human actor)
        {
            var item = subject.Storage.TakeItemOfType<FoodObject>();
            actor.Hunger = 60;
            actor.GiveItem(item);
        }

        public override bool IsPossible(FoodContainer subject, Human actor)
        {
            return subject.Storage.HasItemOfType<FoodObject>();
        }
    }
}