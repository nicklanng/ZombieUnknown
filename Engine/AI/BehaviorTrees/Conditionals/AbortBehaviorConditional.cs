namespace Engine.AI.BehaviorTrees.Conditionals
{
    public class AbortBehaviorConditional : BehaviorConditional
    {
        protected override bool Test(Blackboard blackboard)
        {
            var abortFlag = (bool)blackboard["AbortFlag"];

            return abortFlag;
        }
    }
}
