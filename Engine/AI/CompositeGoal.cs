using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Engine.AI
{
    public abstract class CompositeGoal : Goal
    {
        private readonly Stack<IGoal> _subgoals;

        protected CompositeGoal()
        {
            _subgoals = new Stack<IGoal>();
        }

        public override void Process(GameTime gameTime)
        {
            ProcessSubgoals(gameTime);
        }

        public override void Terminate()
        {
            RemoveAllSubgoals();
        }

        public void AddSubGoal(IGoal goal)
        {
            _subgoals.Push(goal);
        }

        private void ProcessSubgoals(GameTime gameTime)
        {
            if (!_subgoals.Any())
            {
                GoalStatus = GoalStatus.Completed;
                return;
            }

            while (_subgoals.Peek().IsComplete || _subgoals.Peek().HasFailed)
            {
                var finishedSubGoal = _subgoals.Pop();
                finishedSubGoal.Terminate();
            }

            if (!_subgoals.Any())
            {
                GoalStatus = GoalStatus.Completed;
                return;
            }

            _subgoals.Peek().Process(gameTime);

            if (_subgoals.Peek().IsComplete && _subgoals.Count > 1)
            {
                GoalStatus = GoalStatus.Active;
                return;
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
