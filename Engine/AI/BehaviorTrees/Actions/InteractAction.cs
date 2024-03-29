﻿using System.Linq;
using Engine.Entities;

namespace Engine.AI.BehaviorTrees.Actions
{
    public abstract class InteractAction : BehaviorAction
    {
        protected abstract string InteractionText { get; }

        protected override GoalStatus Action(Blackboard blackboard)
        {
            var entity = (MobileEntity)blackboard["subject"];

            var interactionTarget = blackboard.GetValue<PhysicalEntity>("targetEntity");
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

