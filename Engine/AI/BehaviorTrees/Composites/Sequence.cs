namespace Engine.AI.BehaviorTrees.Composites
{
    public class Sequence : BehaviorComposite
    {
        public Sequence(params BehaviorComponent[] behaviors) : base(behaviors) { }

        protected override GoalStatus OnUpdate(Blackboard blackboard)
        {
            foreach (var behavior in Behaviors)
            {
                var result = behavior.Update(blackboard);

                if (result == GoalStatus.Failed)
                {
                    Reset(blackboard);
                    return result;
                }

                if (result == GoalStatus.Active)
                {
                    return result;
                }
            }

            Reset(blackboard);
            return GoalStatus.Completed;
        }
    }
}
