namespace Engine.AI.BehaviorTrees
{
    public class Behavior
    {
        private readonly BehaviorComponent _root;

        public Behavior (BehaviorComponent root)
        {
            _root = root;
        }

        public GoalStatus Update(Blackboard blackboard)
        {
            return _root.Update(blackboard);
        }
    }
}

