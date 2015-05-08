using Engine.AI.Steering;
using Engine.Entities;
using Engine.Maps;
using Engine.Pathfinding;

namespace Engine.AI.BehaviorTrees.Actions
{
    public class CalculateRouteAction : BehaviorAction
    {
        protected override GoalStatus Action(Blackboard blackboard)
        {
            var targetEntity = blackboard.GetValue<ITarget>("targetEntity");
            var aStarSolver = new AStarSolver(
                GameState.PathfindingMap.GetNodeAt(((PhysicalEntity) blackboard["subject"]).MapPosition),
                GameState.PathfindingMap.GetNodeAt(targetEntity.MapPosition)
            );
            var solutionFound = aStarSolver.Solve();
            if (solutionFound == false)
            {
                return GoalStatus.Failed;
            }

            blackboard["movementPath"] = aStarSolver.Solution;

            return GoalStatus.Completed;
        }
    }
}

