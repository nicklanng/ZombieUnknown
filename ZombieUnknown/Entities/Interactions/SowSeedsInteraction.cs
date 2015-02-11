using Engine;
using Engine.Entities.Interactions;
using ZombieUnknown.Entities.Mobiles;
using ZombieUnknown.InventoryObjects;

namespace ZombieUnknown.Entities.Interactions
{
    internal class SowSeedsInteraction : InteractionSingleton<CultivatedLand, Human>
    {
        public static string Text = "Sow Seed";

        public override int MillisToCompleteAction
        {
            get { return 1000; }
        }

        protected override void Execute(CultivatedLand subject, Human actor)
        {
            actor.Rig.GetInventories().TakeItemOfType<WheatSeedObject>();
            GameController.DeleteEntity(subject);
            GameController.SpawnEntity(new Wheat("wheat", subject.MapPosition));
        }

        public override bool IsPossible(CultivatedLand subject, Human actor)
        {
            return actor.Rig.GetInventories().HasItemOfType<WheatSeedObject>();
        }
    }
}