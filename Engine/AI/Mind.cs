using System.Collections.Generic;
using System.Linq;
using Engine.Entities;

namespace Engine.AI
{
    public abstract class Mind<T> where T : Entity
    {
        protected T Entity;
        protected readonly Stack<IGoal> Goals;

        public Mind(T entity)
        {
            Entity = entity;

            Goals = new Stack<IGoal>();
        }

        public virtual void Think()
        {
            if (!Goals.Any())
            {
                return;
            }

            ProcessSubgoals();
        }

        private void ProcessSubgoals()
        {
            if (!Goals.Any())
            {
                return;
            }

            while (Goals.Any() && (Goals.Peek().IsComplete || Goals.Peek().HasFailed))
            {
                var finishedSubGoal = Goals.Pop();
                finishedSubGoal.Terminate();
            }

            if (!Goals.Any())
            {
                return;
            }

            Goals.Peek().Process();

        }
    }
}
