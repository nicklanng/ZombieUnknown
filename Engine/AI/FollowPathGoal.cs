using Engine.Entities;
using Engine.Maps;
using Engine.Pathfinding;

namespace Engine.AI
{
    public class FollowPathGoal : CompositeGoal
    {
        private readonly DrawableEntity _entity;
        private readonly Coordinate _target;

        public FollowPathGoal(DrawableEntity entity, Coordinate target)
        {
            _entity = entity;
            _target = target;
        }

        public override void Activate()
        {
            var aStarSolver = new AStarSolver(GameState.PathfindingMap.GetNodeAt(_entity.GetCoordinate()), GameState.PathfindingMap.GetNodeAt(_target));
            aStarSolver.Solve();
            aStarSolver.Solution.Reverse();

            foreach (var step in aStarSolver.Solution)
            {
                AddSubGoal(new TraverseEdgeGoal(_entity, step));
            }

            base.Activate();
        }
    }
}
