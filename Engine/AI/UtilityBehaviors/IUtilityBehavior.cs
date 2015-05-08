using Engine.AI.BehaviorTrees;
using Engine.Entities;

namespace Engine.AI.UtilityBehaviors
{
    public interface IUtilityBehavior
    {
        BehaviorComponent BehaviorTree { get; }
        float Utility(Entity entity);
    }
}
