using Engine;
using Engine.Entities;
using Engine.Entities.Interactions;

namespace ZombieUnknown.Entities.Interactions
{
    public class HarvestWheatInteraction : Interaction
    {
        public static string Text = "Harvest";
        public override int MillisToCompleteAction { get { return 2000; } }

        public override void Interact(MobileEntity actor, PhysicalEntity subject)
        {
            GameController.DeleteEntity(subject);
            GameController.SpawnEntity(new CultivatedLand("cultivatedLand", subject.MapPosition));
        }
    }
}
