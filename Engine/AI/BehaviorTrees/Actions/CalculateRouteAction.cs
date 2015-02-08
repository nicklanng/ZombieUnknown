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
                GameState.PathfindingMap.GetNodeAt(((PhysicalEntity) blackboard["subject"]).MapPosition), 
                GameState.PathfindingMap.GetNodeAt((Coordinate) blackboard["TargetCoordinate"])
            );
            var solutionFound = aStarSolver.Solve();
            if (solutionFound == false)
            {
                return GoalStatus.Failed;
            }

            blackboard["MovementPath"] = aStarSolver.Solution;

            return GoalStatus.Completed;
        }
    }
}

