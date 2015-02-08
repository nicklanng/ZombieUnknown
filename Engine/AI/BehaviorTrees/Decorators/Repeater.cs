namespace Engine.AI.BehaviorTrees.Decorators
{
    public class Repeater : BehaviorDecorator
    {
        public Repeater(BehaviorComponent behavior) : base(behavior)
        {
        }

        protected override GoalStatus OnUpdate(Blackboard blackboard)
        {
            var result = Behavior.Update(blackboard);

            if (result == GoalStatus.Completed || result == GoalStatus.Failed)
            {
                Reset(blackboard);
            }

            return GoalStatus.Running;
        }
    }
}
