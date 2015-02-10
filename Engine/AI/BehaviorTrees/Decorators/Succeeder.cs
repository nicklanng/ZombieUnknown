namespace Engine.AI.BehaviorTrees.Decorators
{
    public class Succeeder : BehaviorDecorator
    {
        public Succeeder(BehaviorComponent behavior) : base(behavior) { }

        protected override GoalStatus OnUpdate(Blackboard blackboard)
        {
            var result = Behavior.Update(blackboard);

            if (result == GoalStatus.Failed)
            {
                return GoalStatus.Completed;
            }
            
            return result;
        }
    }
}