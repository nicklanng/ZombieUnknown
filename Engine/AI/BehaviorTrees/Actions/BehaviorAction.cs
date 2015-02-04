namespace Engine.AI.BehaviorTrees.Actions
{
    public abstract class BehaviorAction : BehaviorComponent
    {
        protected override GoalStatus OnUpdate(Blackboard blackboard)
        {
            return Action(blackboard);
        }

        protected abstract GoalStatus Action(Blackboard blackboard);
    }
}

