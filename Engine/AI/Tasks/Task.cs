using Engine.Entities;

namespace Engine.AI.Tasks
{
    public abstract class Task<T> : ITask where T : PhysicalEntity
    {
        public PhysicalEntity Assigned { get; private set; }

        public string Action { get; private set; }
        public PhysicalEntity Target { get; private set; }

        public bool IsComplete { get; private set; }

        protected Task(T target, string action)
        {
            Target = target;
            Action = action;

            target.Deleted += (i, o) => Complete();
        }

        public bool IsAvailable()
        {
            return Assigned == null;
        }

        public void Pick(PhysicalEntity actor)
        {
            GameState.TaskList.ReleaseTasks(actor);
            Assigned = actor;
        }

        public void PutBack()
        {
            Assigned = null;
        }

        public void Complete()
        {
            IsComplete = true;
            GameState.TaskList.Remove(this);
        }
    }
}
