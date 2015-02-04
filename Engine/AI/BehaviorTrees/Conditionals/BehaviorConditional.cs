namespace Engine.AI.BehaviorTrees.Conditionals
{
    public abstract class BehaviorConditional : BehaviorComponent
    {
        protected override GoalStatus OnUpdate(Blackboard blackboard)
        {
            var result = Test(blackboard);

            return result ? GoalStatus.Completed : GoalStatus.Failed;
        }

        protected abstract bool Test(Blackboard blackboard);
    }
}
