using System;

namespace Engine.AI.BehaviorTrees
{
	public abstract class BehaviorComponent
	{
        public abstract GoalStatus Update();
	}
}
