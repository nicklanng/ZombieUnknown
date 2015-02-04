using System;

namespace Engine.AI.BehaviorTrees
{
    public abstract class BehaviorAction : BehaviorComponent
    {
        protected Blackboard Blackboard;

        public BehaviorAction(Blackboard blackboard)
        {
            Blackboard = blackboard;
        }

        public override GoalStatus Update()
        {
            return Action();
        }

        protected abstract GoalStatus Action();
    }
}

