namespace Engine.AI.BehaviorTrees
{
    public abstract class Conditional : BehaviorComponent
    {
        protected Blackboard Blackboard;

        protected Conditional(Blackboard blackboard)
        {
            Blackboard = blackboard;
        }

        public override GoalStatus Update()
        {
            return Test();
        }

        protected abstract GoalStatus Test();
    }
}
