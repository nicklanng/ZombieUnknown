namespace Engine.AI.BehaviorTrees
{
    public abstract class BehaviorAction : BehaviorComponent
    {
        protected Blackboard Blackboard;

        protected BehaviorAction(Blackboard blackboard)
        {
            Blackboard = blackboard;
        }

        public sealed override GoalStatus Update()
        {
            return Action();
        }

        protected abstract GoalStatus Action();
    }
}

