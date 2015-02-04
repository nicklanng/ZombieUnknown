using System;
using System.Linq;
using Engine.Entities;

namespace Engine.AI.BehaviorTrees.Actions
{
    public class InteractAction : BehaviorAction
    {
        protected override GoalStatus Action(Blackboard blackboard)
        {
            var entity = blackboard ["Entity"] as PhysicalEntity;
            var interactionTarget = blackboard["InteractionTarget"] as IInteractable;
            var accessPositions = interactionTarget.AccessPositions;

            var accessPosition = accessPositions.SingleOrDefault(x => (x.PositionOffset + interactionTarget.MapPosition) == entity.MapPosition);
            if (accessPosition == null) 
            {
                return GoalStatus.Failed;
            }

            var requiredDirection = accessPosition.Direction;
            entity.SetAnimation ("idle");
            entity.FaceDirection(requiredDirection);

            return GoalStatus.Active;
        }
    }
}

