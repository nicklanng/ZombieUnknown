namespace Engine.AI.BehaviorTrees.Decorators
{
    class Inverter : BehaviorDecorator
    {
        public Inverter(BehaviorComponent behavior) : base(behavior) { }

        protected override GoalStatus OnUpdate(Blackboard blackboard)
        {
            var result = Behavior.Update(blackboard);

            if (result == GoalStatus.Failed)
            {
                return GoalStatus.Completed;
            }

            if (result == GoalStatus.Completed)
            {
                return GoalStatus.Failed;
            }

            return result;
        }
    }
}
