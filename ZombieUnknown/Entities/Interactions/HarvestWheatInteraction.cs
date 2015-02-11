using Engine;
using Engine.Entities.Interactions;
using ZombieUnknown.Entities.Mobiles;
using ZombieUnknown.InventoryObjects;

namespace ZombieUnknown.Entities.Interactions
{
    internal class HarvestWheatInteraction : InteractionSingleton<Wheat, Human>
    {
        public static string Text = "Harvest Wheat";

        public override int MillisToCompleteAction
        {
            get { return 1000; }
        }

        protected override void Execute(Wheat subject, Human actor)
        {
            actor.Rig.GetInventories().AddItem(new WheatSeedObject());
            actor.Rig.GetInventories().AddItem(new WheatObject());
            GameController.DeleteEntity(subject);
            GameController.SpawnEntity(new CultivatedLand("cultivatedLand", subject.MapPosition));
        }

        public override bool IsPossible(Wheat subject, Human actor)
        {
            return subject.IsGrown;
        }
    }
}