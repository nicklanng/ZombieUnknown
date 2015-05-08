namespace Engine.AI.BehaviorTrees
{
    public class Behavior
    {
        private readonly BehaviorComponent _root;

        public GoalStatus CurrentStatus { get; private set; }

        public Behavior (BehaviorComponent root)
        {
            _root = root;
        }

        public GoalStatus Update(Blackboard blackboard)
        {
            CurrentStatus = _root.Update(blackboard);
            return CurrentStatus;
        }
    }
}

