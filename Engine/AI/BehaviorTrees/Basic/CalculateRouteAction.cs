using System;
using Engine.Pathfinding;
using Engine.Maps;
using Engine.Entities;

namespace Engine.AI.BehaviorTrees.Basic
{
    public class CalculateRouteAction : BehaviorAction
    {
        public CalculateRouteAction(Blackboard blackboard)
            : base(blackboard)
        {   
        }

        protected override GoalStatus Action ()
        {
            var aStarSolver = new AStarSolver(
                GameState.PathfindingMap.GetNodeAt((Blackboard["Entity"] as DrawableEntity).GetCoordinate()), 
                GameState.PathfindingMap.GetNodeAt(Blackboard["TargetCoordinate"] as Coordinate)
            );
            aStarSolver.Solve();

            Blackboard["MovementPath"] = aStarSolver.Solution;

            return GoalStatus.Completed;
        }
    }
}

