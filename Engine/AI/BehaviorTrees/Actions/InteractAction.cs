using System.Linq;
using Engine.Entities.Interactions;

namespace Engine.AI.BehaviorTrees.Actions
{
    public abstract class InteractAction : BehaviorAction
    {
        protected override GoalStatus Action(Blackboard blackboard)
        {
            var targetInteraction = (ITargetInteraction)blackboard["TargetInteraction"];
            if (SavedResult == GoalStatus.Inactive)
            {
                var accessPositions = targetInteraction.Subject.AccessPositions;

                var accessPosition = accessPositions.SingleOrDefault(x => (x.PositionOffset + targetInteraction.Subject.MapPosition) == targetInteraction.Actor.MapPosition);
                if (accessPosition == null)
                {
                    return GoalStatus.Failed;
                }

                var requiredDirection = accessPosition.Direction;
                targetInteraction.Actor.TransitionState("interact");
                targetInteraction.Actor.FaceDirection(requiredDirection);

                blackboard["TimeWhenInteractionFinished"] = GameState.GameTime.TotalGameTime.TotalMilliseconds + targetInteraction.MillisToCompleteAction;

                return GoalStatus.Running;
            }

            if (SavedResult == GoalStatus.Running)
            {
                var timeWhenInteractionFinished = (double)blackboard["TimeWhenInteractionFinished"];
                var timeNow = GameState.GameTime.TotalGameTime.TotalMilliseconds;

                if (timeNow >= timeWhenInteractionFinished)
                {
                    targetInteraction.Actor.TransitionState("idle");
                    targetInteraction.Interact();

                    return GoalStatus.Completed;
                }
            }

            return GoalStatus.Running;
        }
    }
}