﻿using System.Linq;
using Engine.Entities;

namespace Engine.AI.BehaviorTrees.Actions
{
    public class InteractAction : BehaviorAction
    {
        protected override GoalStatus Action(Blackboard blackboard)
        {
            var entity = (PhysicalEntity)blackboard["Entity"];

            if (SavedResult == GoalStatus.Inactive)
            {
                var interactionTarget = (IInteractable)blackboard["InteractionTarget"];
                var accessPositions = interactionTarget.AccessPositions;

                var accessPosition = accessPositions.SingleOrDefault(x => (x.PositionOffset + interactionTarget.MapPosition) == entity.MapPosition);
                if (accessPosition == null) 
                {
                    return GoalStatus.Failed;
                }

                var requiredDirection = accessPosition.Direction;
                entity.SetAnimation ("interact");
                entity.FaceDirection(requiredDirection);

                var interactionAction = interactionTarget.Interactions[0];

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
                    return GoalStatus.Completed;
                }
            }
            
            return GoalStatus.Active;
        }
    }
}

