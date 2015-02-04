using System;

namespace Engine.AI.BehaviorTrees
{
	public abstract class BehaviorComponent
	{
        protected Guid Guid = Guid.NewGuid();

        public GoalStatus GoalStatus { get; protected set; }

	    public GoalStatus Update(Blackboard blackboard)
	    {
	        var treeState = blackboard.TreeStatus;
	        if (treeState.ContainsKey(Guid))
	        {
	            var savedResult = treeState[Guid];

	            if (savedResult == GoalStatus.Completed || savedResult == GoalStatus.Failed)
	            {
                    return savedResult;
	            }
	        }

	        var result = OnUpdate(blackboard);
            blackboard.TreeStatus[Guid] = result;
	        return result;
	    }

	    public virtual void Reset(Blackboard blackboard)
	    {
	        var treeState = blackboard.TreeStatus;
	        if (treeState.ContainsKey(Guid))
	        {
	            treeState.Remove(Guid);
	        }
	    }

	    protected abstract GoalStatus OnUpdate(Blackboard blackboard);
	}
}
