using Engine.Entities;
using Engine.Maps;
using Engine.Pathfinding;

namespace Engine.AI.BehaviorTrees.Actions
{
    public class CalculateRouteAction : BehaviorAction
    {
        protected override GoalStatus Action(Blackboard blackboard)
        {
            var aStarSolver = new AStarSolver(
                GameState.PathfindingMap.GetNodeAt(((PhysicalEntity) blackboard["Entity"]).MapPosition), 
                GameState.PathfindingMap.GetNodeAt((Coordinate) blackboard["TargetCoordinate"])
            );
            aStarSolver.Solve();

            blackboard["MovementPath"] = aStarSolver.Solution;

            return GoalStatus.Completed;
        }
    }
}

