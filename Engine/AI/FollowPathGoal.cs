using Engine.Entities;
using Engine.Maps;
using Engine.Pathfinding;

namespace Engine.AI
{
    public class FollowPathGoal : CompositeGoal
    {
        private readonly PhysicalEntity _entity;
        private readonly Coordinate _target;
        private readonly bool _run;

        public FollowPathGoal(PhysicalEntity entity, Coordinate target, bool run = false)
        {
            _entity = entity;
            _target = target;
            _run = run;
        }

        public override void Activate()
        {
            base.Activate();

            _entity.IsRunning = _run;

            var aStarSolver = new AStarSolver(GameState.PathfindingMap.GetNodeAt(_entity.MapPosition), GameState.PathfindingMap.GetNodeAt(_target));
            aStarSolver.Solve();

            foreach (var step in aStarSolver.Solution)
            {
                AddSubGoal(new TraverseEdgeGoal(_entity, step, _run));
            }
        }

        public override void Terminate()
        {
            base.Terminate();
            _entity.IsRunning = false;

            if (GoalStatus == GoalStatus.Failed)
            {
                AddSubGoal(new WaitGoal(_entity, 100));
            }
        }
    }
}
