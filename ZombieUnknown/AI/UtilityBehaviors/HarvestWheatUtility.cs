using System.Linq;
using Engine;
using Engine.AI.BehaviorTrees;
using Engine.AI.BehaviorTrees.Actions;
using Engine.AI.BehaviorTrees.Composites;
using Engine.AI.UtilityBehaviors;
using Engine.Entities;
using ZombieUnknown.AI.BehaviorTrees.Conditionals;
using ZombieUnknown.AI.Tasks;
using ZombieUnknown.Entities.Mobiles;

namespace ZombieUnknown.AI.UtilityBehaviors
{
    class HarvestWheatUtility : IUtilityBehavior
    {
        public BehaviorComponent BehaviorTree { get; private set; }

        public HarvestWheatUtility()
        {
            BehaviorTree = 
                new Sequence(
                    new GetTaskAction<HarvestWheatTask>(),
                    new HasTaskConditional<HarvestWheatTask>(),
                    new CalculateRouteAction(),
                    new MoveTowardsTarget(),
                    new PerformTask(),
                    new CompleteTask()
                );
        }

        public float Utility(Entity entity)
        {
            var human = (Human) entity;
            if (human == null) return 0.0f;

            var tasks = GameState.TaskList
                .GetAllOfType<HarvestWheatTask>()
                .Where(x => x.IsAvailable() || x.Assigned == human);

            if (!tasks.Any()) return 0.0f;

            return 1.0f;
        }

    }
}
