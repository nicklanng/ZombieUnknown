using Engine.Entities;

namespace Engine.AI.BehaviorTrees.Actions
{
    public class IdleAction : BehaviorAction
    {
        protected override GoalStatus Action(Blackboard blackboard)
        {
            var entity = (PhysicalEntity)blackboard["subject"];
            entity.TransitionState("idle");

            return GoalStatus.Completed;
        }
    }
}
