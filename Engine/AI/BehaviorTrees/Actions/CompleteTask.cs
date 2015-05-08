using Engine.AI.Tasks;

namespace Engine.AI.BehaviorTrees.Actions
{
    public class CompleteTask : BehaviorAction
    {
        protected override GoalStatus Action(Blackboard blackboard)
        {
            var task = blackboard.GetValue<ITask>("currentTask");
            if (task == null) return GoalStatus.Failed;
            task.Complete();
            blackboard["currentTask"] = null;
            return GoalStatus.Completed;
        }
    }
}
