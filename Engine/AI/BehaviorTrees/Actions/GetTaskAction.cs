using System.Linq;
using Engine.AI.Tasks;
using Engine.Entities;
using Engine.Extensions;

namespace Engine.AI.BehaviorTrees.Actions
{
    public class GetTaskAction<T> : BehaviorAction where T : ITask
    {
        protected override GoalStatus Action(Blackboard blackboard)
        {
            var entity = (PhysicalEntity)blackboard["subject"];
            var tasks = GameState.TaskList
                .GetAllOfType<T>()
                .Where(x => x.IsAvailable() || x.Assigned == entity)
                .ToList();

            if (!tasks.Any()) return GoalStatus.Failed;

            var task = tasks.OrderBy(x => entity.MapPosition.DistanceTo(x.Target.MapPosition)).First();
            task.Pick(entity);
            blackboard["currentTask"] = task;
            blackboard["targetEntity"] = task.Target;

            return GoalStatus.Completed;
        }
    }
}
