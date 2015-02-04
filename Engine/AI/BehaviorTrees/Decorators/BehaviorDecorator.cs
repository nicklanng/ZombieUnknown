namespace Engine.AI.BehaviorTrees.Decorators
{
    public abstract class BehaviorDecorator : BehaviorComponent
    {
        protected readonly BehaviorComponent Behavior;

        protected BehaviorDecorator(BehaviorComponent behavior)
        {
            Behavior = behavior;
        }

        public override void Reset(Blackboard blackboard)
        {
            base.Reset(blackboard);
            Behavior.Reset(blackboard);
        }
    }
}