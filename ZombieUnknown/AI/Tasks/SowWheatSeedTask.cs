using Engine.AI.Tasks;
using ZombieUnknown.Entities;

namespace ZombieUnknown.AI.Tasks
{
    public class SowWheatSeedTask : Task<CultivatedLand>
    {
        public SowWheatSeedTask(CultivatedLand target)
            : base(target, "Sow Seed")
        {
        }
    }
}
