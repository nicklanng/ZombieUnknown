using System.Linq;
using Engine.Entities;
using Microsoft.Xna.Framework;

namespace Engine.AI.BehaviorTrees.Actions
{
    public abstract class InteractAction : BehaviorAction
    {
        protected abstract string InteractionText { get; }

        protected override GoalStatus Action(Blackboard blackboard)
        {
            var actor = (MobileEntity)blackboard["subject"];

            var interactionTargetLocation = (Vector2)blackboard["InteractionTargetLocation"];
            var entities = GameState.Map.GetEntitiesAt(interactionTargetLocation);
            var subject = (PhysicalEntity)entities.Last(x => x is PhysicalEntity);
            if (subject == null) 
            {
                return GoalStatus.Failed;
            }

            if (subject.Interactions.ContainsKey(InteractionText) == false)
            {
                return GoalStatus.Failed;
            }
            var interactionAction = subject.Interactions[InteractionText];

            if (SavedResult == GoalStatus.Inactive)
            {
                var accessPositions = subject.AccessPositions;

                var accessPosition = accessPositions.SingleOrDefault(x => (x.PositionOffset + subject.MapPosition) == actor.MapPosition);
                if (accessPosition == null) 
                {
                    return GoalStatus.Failed;
                }

                var requiredDirection = accessPosition.Direction;
                actor.TransitionState("interact");
                actor.FaceDirection(requiredDirection);


                blackboard["TimeWhenInteractionFinished"] = GameState.GameTime.TotalGameTime.TotalMilliseconds + interactionAction.MillisToCompleteAction;

                return GoalStatus.Running;
            }

            if (SavedResult == GoalStatus.Running)
            {
                var timeWhenInteractionFinished = (double)blackboard["TimeWhenInteractionFinished"];
                var timeNow = GameState.GameTime.TotalGameTime.TotalMilliseconds;

                if (timeNow >= timeWhenInteractionFinished)
                {
                    actor.TransitionState("idle");
                    interactionAction.Interact(subject, actor);

                    return GoalStatus.Completed;
                }
            }
            
            return GoalStatus.Running;
        }
    }
}

