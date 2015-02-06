using Engine;
using Engine.Entities;
using Engine.Entities.Interactions;

namespace ZombieUnknown.Entities.Interactions
{
    class SowSeedInteraction : IInteraction
    {
        private readonly PhysicalEntity _entity;

        public static string Text = "Sow Seed";

        public int MillisToCompleteAction { get { return 1000; } }

        public SowSeedInteraction(PhysicalEntity entity)
        {
            _entity = entity;
        }

        public void Interact(PhysicalEntity entity)
        {
            GameController.DeleteEntity(_entity);
            GameController.SpawnEntity(new Wheat("wheat", _entity.MapPosition));
        }
    }
}
