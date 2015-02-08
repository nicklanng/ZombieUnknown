using Engine;
using Engine.Entities;
using Engine.Entities.Interactions;

namespace ZombieUnknown.Entities.Interactions
{
    class SowSeedInteraction : Interaction
    {
        public static string Text = "Sow Seed";

        public override int MillisToCompleteAction { get { return 1000; } }

        public SowSeedInteraction(PhysicalEntity subject) : base(subject)
        {
        }

        public override void Interact(PhysicalEntity entity)
        {
            GameController.DeleteEntity(Subject);
            GameController.SpawnEntity(new Wheat("wheat", Subject.MapPosition));
        }
    }
}
