using System.Collections.Generic;
using System.Linq;
using Engine.Entities;

namespace Engine.AI.Tasks
{
    public class TaskList
    {
        private readonly List<ITask> _tasks;

        public TaskList()
        {
            _tasks = new List<ITask>();
        }

        public void AddTask(ITask task)
        {
            _tasks.Add(task);
        }

        public IEnumerable<T> GetAllOfType<T>()
        {
            return _tasks.OfType<T>();
        }

        public void Remove(ITask task)
        {
            _tasks.Remove(task);
        }

        public void ReleaseTasks(PhysicalEntity actor)
        {
            var assignedTasks = _tasks.Where(x => x.Assigned == actor);
            foreach (var assignedTask in assignedTasks)
            {
                assignedTask.PutBack();
            }
        }
    }
}
