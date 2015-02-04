namespace Engine.AI.BehaviorTrees.Composites
{
    public abstract class BehaviorComposite : BehaviorComponent
    {
        protected readonly BehaviorComponent[] Behaviors;

        protected BehaviorComposite(params BehaviorComponent[] behaviors)
        {
            Behaviors = behaviors;
        }

        public override void Reset(Blackboard blackboard)
        {
            base.Reset(blackboard);

            foreach (var behavior in Behaviors)
            {
                behavior.Reset(blackboard);
            }
        }
    }
}