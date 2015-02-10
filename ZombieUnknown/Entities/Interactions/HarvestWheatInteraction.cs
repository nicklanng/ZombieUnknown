using Engine;
using Engine.Entities;
using Engine.Entities.Interactions;
using ZombieUnknown.Entities.Mobiles;
using ZombieUnknown.InventoryObjects;

namespace ZombieUnknown.Entities.Interactions
{
    internal class HarvestWheatInteraction : Interaction
    {
        public static string Text = "Harvest Wheat";

        public override int MillisToCompleteAction
        {
            get { return 1000; }
        }
        
        public override void Interact(PhysicalEntity subject, PhysicalEntity actor)
        {
            if (IsPossible(actor))
            {
                var humanActor = (Human)actor;

                humanActor.Rig.GetInventories().AddItem(new WheatSeedObject());
                humanActor.Rig.GetInventories().AddItem(new WheatObject());
                GameController.DeleteEntity(subject);
                GameController.SpawnEntity(new CultivatedLand("cultivatedLand", subject.MapPosition));
            }
        }

        public override bool IsPossible(PhysicalEntity actor)
        {
            return true;
        }
    }
}