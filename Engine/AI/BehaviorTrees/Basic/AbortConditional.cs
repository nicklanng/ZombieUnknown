namespace Engine.AI.BehaviorTrees.Basic
{
    public class AbortConditional : Conditional
    {
        public AbortConditional(Blackboard blackboard) 
            : base(blackboard)
        {
        }

        protected override GoalStatus Test()
        {
            var abortFlag = (bool)Blackboard["AbortFlag"];

            return abortFlag ? GoalStatus.Failed : GoalStatus.Completed;
        }
    }
}
