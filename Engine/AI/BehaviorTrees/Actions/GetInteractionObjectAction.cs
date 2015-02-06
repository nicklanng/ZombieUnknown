﻿using System;
using System.Linq;
using Engine.Maps;

namespace Engine.AI.BehaviorTrees.Actions
{
    public class GetInteractionObjectAction : BehaviorAction
    {
        protected override GoalStatus Action(Blackboard blackboard)
        {
            blackboard["InteractionTargetLocation"] = GameState.InteractionObject.MapPosition;
            blackboard["TargetCoordinate"] = (Coordinate)(GameState.InteractionObject.MapPosition + GameState.InteractionObject.AccessPositions.First().PositionOffset);

            return GoalStatus.Completed;
        }
    }
}

