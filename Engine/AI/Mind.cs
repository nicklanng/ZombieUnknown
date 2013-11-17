using System.Collections.Generic;
using System.Linq;
using Engine.Entities;
using Engine.Maps;
using Microsoft.Xna.Framework;

namespace Engine.AI
{
    public class Mind<T> where T : Entity
    {
        private T _entity;
        private readonly Stack<IGoal> _subgoals;

        public Mind(T entity)
        {
            _entity = entity;

            _subgoals = new Stack<IGoal>();

            _subgoals.Push(new FollowPathGoal(_entity as MoveableEntity, new Coordinate(1, 1)));
            _subgoals.Push(new FollowPathGoal(_entity as MoveableEntity, new Coordinate(14, 19)));
        }

        public void Think()
        {
            if (!_subgoals.Any())
            {
                return;
            }

            ProcessSubgoals();
        }

        private void ProcessSubgoals()
        {
            if (!_subgoals.Any())
            {
                return;
            }

            while (_subgoals.Any() && (_subgoals.Peek().IsComplete || _subgoals.Peek().HasFailed))
            {
                var finishedSubGoal = _subgoals.Pop();
                finishedSubGoal.Terminate();
            }

            if (!_subgoals.Any())
            {
                return;
            }

            _subgoals.Peek().Process();

        }
    }
}
