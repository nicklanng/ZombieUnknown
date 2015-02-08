using Engine;
using Engine.AI;
using Engine.AI.BehaviorTrees;
using Engine.AI.BehaviorTrees.Actions;
using Engine.Entities;
using Engine.Maps;
using Engine.Pathfinding;

namespace ZombieUnknown.AI.BehaviorTrees.Actions
{
    class ZombieCalculateRouteAction : BehaviorAction
    {
        protected override GoalStatus Action(Blackboard blackboard)
        {
            var aStarSolver = new AStarSolver(
                GameState.PathfindingMap.GetNodeAt(((PhysicalEntity)blackboard["subject"]).MapPosition),
                GameState.PathfindingMap.GetNodeAt((Coordinate)blackboard["TargetCoordinate"]),
                2
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
