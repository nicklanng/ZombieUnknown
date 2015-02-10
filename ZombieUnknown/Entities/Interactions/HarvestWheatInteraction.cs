using Engine;
using Engine.Entities.Interactions;
using ZombieUnknown.Entities.Mobiles;
using ZombieUnknown.InventoryObjects;

namespace ZombieUnknown.Entities.Interactions
{
    internal class HarvestWheatInteraction : IInteraction<Wheat, Human>
    {
        public static string Text = "Harvest Wheat";

        public int MillisToCompleteAction
        {
            get { return 1000; }
        }

        public void Interact(Wheat subject, Human actor)
        {
            if (IsPossible(subject, actor))
            {
                actor.Rig.GetInventories().AddItem(new WheatSeedObject());
                actor.Rig.GetInventories().AddItem(new WheatObject());
                GameController.DeleteEntity(subject);
                GameController.SpawnEntity(new CultivatedLand("cultivatedLand", subject.MapPosition));
            }
        }

        public bool IsPossible(Wheat subject, Human actor)
        {
            return subject.IsGrown;
        }
    }
}