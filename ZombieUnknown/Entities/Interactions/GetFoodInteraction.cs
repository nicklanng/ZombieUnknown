using Engine.Entities.Interactions;

namespace ZombieUnknown.Entities.Interactions
{
    public class GetFoodInteraction : IInteraction
    {
        public int MillisToCompleteAction { get { return 2000; } }
    }
}
