using Engine.AI.BehaviorTrees;
using Engine.AI.BehaviorTrees.Conditionals;
using Engine.AI.Tasks;

namespace ZombieUnknown.AI.BehaviorTrees.Conditionals
{
    class HasTaskConditional<T> : BehaviorConditional where T : ITask
    {
        public HasTaskConditional()
        {
            EvaluateEveryTime = true;
        }

        protected override bool Test(Blackboard blackboard)
        {
            var currentTask = blackboard.GetValue<T>("currentTask");

            if (currentTask == null) return false;

            if (currentTask.IsComplete)
            {
                blackboard["currentTask"] = null;
                return false;
            }

            return true;
        }
    }
}
