using System.Linq;
using Engine.Entities.Interactions;

namespace Engine.AI.BehaviorTrees.Actions
{
    public abstract class InteractAction : BehaviorAction
    {
        protected override GoalStatus Action(Blackboard blackboard)
        {
            var targetedInteraction = (ITargetedInteraction)blackboard["TargetInteraction"];
            if (SavedResult == GoalStatus.Inactive)
            {
                var accessPositions = targetedInteraction.Subject.AccessPositions;

                var accessPosition = accessPositions.SingleOrDefault(x => (x.PositionOffset + targetedInteraction.Subject.MapPosition) == targetedInteraction.Actor.MapPosition);
                if (accessPosition == null)
                {
                    return GoalStatus.Failed;
                }

                var requiredDirection = accessPosition.Direction;
                targetedInteraction.Actor.TransitionState("interact");
                targetedInteraction.Actor.FaceDirection(requiredDirection);

                blackboard["TimeWhenInteractionFinished"] = GameState.GameTime.TotalGameTime.TotalMilliseconds + targetedInteraction.MillisToCompleteAction;

                return GoalStatus.Running;
            }

            if (SavedResult == GoalStatus.Running)
            {
                var timeWhenInteractionFinished = (double)blackboard["TimeWhenInteractionFinished"];
                var timeNow = GameState.GameTime.TotalGameTime.TotalMilliseconds;

                if (timeNow >= timeWhenInteractionFinished)
                {
                    targetedInteraction.Actor.TransitionState("idle");
                    targetedInteraction.Interact();

                    return GoalStatus.Completed;
                }
            }

            return GoalStatus.Running;
        }
    }
}