using System.Linq;
using Engine.Entities;
using Engine.Maps;
using Microsoft.Xna.Framework;

namespace Engine.AI.BehaviorTrees.Actions
{
    public abstract class InteractAction : BehaviorAction
    {
        protected abstract string InteractionText { get; }

        protected override GoalStatus Action(Blackboard blackboard)
        {
            var entity = (PhysicalEntity)blackboard["Entity"];

            var interactionTargetLocation = (Vector2)blackboard["InteractionTargetLocation"];
            var entities = GameState.Map.GetEntitiesAt(interactionTargetLocation);
            var interactionTarget = (IInteractable)entities.SingleOrDefault(x => (IInteractable)x != null);
            if (interactionTarget == null) 
            {
                return GoalStatus.Failed;
            }

            if (interactionTarget.Interactions.ContainsKey(InteractionText) == false)
            {
                return GoalStatus.Failed;
            }
            var interactionAction = interactionTarget.Interactions[InteractionText];

            if (SavedResult == GoalStatus.Inactive)
            {
                var accessPositions = interactionTarget.AccessPositions;

                var accessPosition = accessPositions.SingleOrDefault(x => (x.PositionOffset + interactionTarget.MapPosition) == entity.MapPosition);
                if (accessPosition == null) 
                {
                    return GoalStatus.Failed;
                }

                var requiredDirection = accessPosition.Direction;
                entity.SetAnimation ("interact");
                entity.FaceDirection(requiredDirection);


                blackboard["TimeWhenInteractionFinished"] = GameState.GameTime.TotalGameTime.TotalMilliseconds + interactionAction.MillisToCompleteAction;

                return GoalStatus.Active;
            }

            if (SavedResult == GoalStatus.Active)
            {
                var timeWhenInteractionFinished = (double)blackboard["TimeWhenInteractionFinished"];
                var timeNow = GameState.GameTime.TotalGameTime.TotalMilliseconds;

                if (timeNow >= timeWhenInteractionFinished)
                {
                    entity.SetAnimation("idle");
                    interactionAction.Interact(entity);

                    return GoalStatus.Completed;
                }
            }
            
            return GoalStatus.Active;
        }
    }
}

