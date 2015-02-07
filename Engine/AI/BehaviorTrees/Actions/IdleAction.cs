using Engine.Entities;

namespace Engine.AI.BehaviorTrees.Actions
{
    public class IdleAction : BehaviorAction
    {
        protected override GoalStatus Action(Blackboard blackboard)
        {
            var entity = (PhysicalEntity)blackboard["Entity"];
            entity.TransitionState("idle");

            return GoalStatus.Completed;
        }
    }
}
