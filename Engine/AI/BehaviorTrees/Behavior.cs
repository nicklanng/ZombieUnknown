using System;

namespace Engine.AI.BehaviorTrees
{
    public class Behavior
    {
        private BehaviorComponent _root;

        public Behavior (BehaviorComponent root)
        {
            _root = root;
        }

        public GoalStatus Update()
        {
            return _root.Update();
        }
    }
}

