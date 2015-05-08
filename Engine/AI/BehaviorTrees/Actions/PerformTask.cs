using Engine.AI.Tasks;
using Engine.Entities;

namespace Engine.AI.BehaviorTrees.Actions
{
    public class PerformTask : BehaviorAction
    {
        protected override GoalStatus Action(Blackboard blackboard)
        {
            var entity = blackboard.GetValue<MobileEntity>("subject");
            var task = blackboard.GetValue<ITask>("currentTask");

            var interactionTarget = blackboard.GetValue<PhysicalEntity>("targetEntity");
            if (interactionTarget == null)
            {
                return GoalStatus.Failed;
            }

            if (interactionTarget.Interactions.ContainsKey(task.Action) == false)
            {
                return GoalStatus.Failed;
            }
            var interactionAction = interactionTarget.Interactions[task.Action];

            if (SavedResult == GoalStatus.Inactive)
            {
                entity.TransitionState("interact");

                blackboard["TimeWhenInteractionFinished"] = GameState.GameTime.TotalGameTime.TotalMilliseconds + interactionAction.MillisToCompleteAction;

                return GoalStatus.Running;
            }

            if (SavedResult == GoalStatus.Running)
            {
                var timeWhenInteractionFinished = (double)blackboard["TimeWhenInteractionFinished"];
                var timeNow = GameState.GameTime.TotalGameTime.TotalMilliseconds;

                if (timeNow >= timeWhenInteractionFinished)
                {
                    entity.TransitionState("idle");
                    interactionAction.Interact(entity, interactionTarget);

                    return GoalStatus.Completed;
                }
            }

            return GoalStatus.Running;
        }
    }
}
