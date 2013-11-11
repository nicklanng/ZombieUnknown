using System.Collections.Generic;
using System.Linq;

namespace Engine.AI
{
    public abstract class CompositeGoal : Goal
    {
        private readonly Stack<IGoal> _subgoals;

        protected CompositeGoal()
        {
            _subgoals = new Stack<IGoal>();
        }

        public override void Process()
        {
            ProcessSubgoals();
        }

        public override void Terminate()
        {
            RemoveAllSubgoals();
        }

        public void AddSubGoal(IGoal goal)
        {
            _subgoals.Push(goal);
        }

        private void ProcessSubgoals()
        {
            if (!_subgoals.Any())
            {
                GoalStatus = GoalStatus.Completed;
            }

            while (_subgoals.Peek().IsComplete() || _subgoals.Peek().HasFailed())
            {
                var finishedSubGoal = _subgoals.Pop();
                finishedSubGoal.Terminate();
            }

            if (!_subgoals.Any())
            {
                GoalStatus = GoalStatus.Completed;
            }

            if (_subgoals.Peek().IsComplete() && _subgoals.Count > 1)
            {
                GoalStatus = GoalStatus.Active;
            }

            GoalStatus = _subgoals.Peek().GoalStatus;
        }

        private void RemoveAllSubgoals()
        {
            while (_subgoals.Any())
            {
                var subgoal = _subgoals.Pop();
                subgoal.Terminate();
            }
        }
    }
}
