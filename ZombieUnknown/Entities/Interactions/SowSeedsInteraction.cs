using Engine;
using Engine.Entities.Interactions;
using ZombieUnknown.Entities.Mobiles;
using ZombieUnknown.InventoryObjects;

namespace ZombieUnknown.Entities.Interactions
{
    internal class SowSeedsInteraction : IInteraction<CultivatedLand, Human>
    {
        public static string Text = "Sow Seed";

        public int MillisToCompleteAction
        {
            get { return 1000; }
        }

        public void Interact(CultivatedLand subject, Human actor)
        {
            if (IsPossible(subject, actor))
            {
                actor.Rig.GetInventories().TakeItemOfType<WheatSeedObject>();
                GameController.DeleteEntity(subject);
                GameController.SpawnEntity(new Wheat("wheat", subject.MapPosition));
            }
        }

        public bool IsPossible(CultivatedLand subject, Human actor)
        {
            return actor.Rig.GetInventories().HasItemOfType<WheatSeedObject>();
        }
    }
}