namespace Engine.AI.BehaviorTrees.Decorators
{
    public class RepeaterUntilFail : BehaviorDecorator
    {
        public RepeaterUntilFail(BehaviorComponent behavior) : base(behavior) { }

        protected override GoalStatus OnUpdate(Blackboard blackboard)
        {
            var result = Behavior.Update(blackboard);

            if (result == GoalStatus.Completed)
            {
                Behavior.Reset(blackboard);
            }

            if (result == GoalStatus.Failed)
            {
                return result;
            }

            return GoalStatus.Active;
        }
    }
}
