using Engine.Entities;
using Engine.Entities.Interactions;

namespace ZombieUnknown.Entities.Interactions
{
    public class GetFoodInteraction : IInteraction
    {
        public static string Text = "Get Food";

        public int MillisToCompleteAction { get { return 2000; } }

        public void Interact(PhysicalEntity entity)
        {
            var human = (Human)entity;
            if (human != null)
            {
                human.Hunger = 60;
            }
        }
    }
}
