using Engine.Entities;

namespace Engine.AI.BehaviorTrees.Actions
{
    public class IdleAction : BehaviorAction
    {
        protected override GoalStatus Action(Blackboard blackboard)
        {
            var entity = (PhysicalEntity)blackboard["Entity"];
            entity.SetAnimation("idle");

            return GoalStatus.Completed;
        }
    }
}
