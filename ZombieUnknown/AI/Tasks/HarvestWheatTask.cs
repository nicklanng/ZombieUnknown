using Engine.AI.Tasks;
using ZombieUnknown.Entities;

namespace ZombieUnknown.AI.Tasks
{
    public class HarvestWheatTask : Task<Wheat>
    {
        public HarvestWheatTask(Wheat target) 
            : base(target, "Harvest")
        {
        }
    }
}
