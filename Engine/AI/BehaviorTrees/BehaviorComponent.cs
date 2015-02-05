using System;

namespace Engine.AI.BehaviorTrees
{
	public abstract class BehaviorComponent
	{
        protected Guid Guid = Guid.NewGuid();

        protected GoalStatus SavedResult { get; set; }

	    public GoalStatus Update(Blackboard blackboard)
	    {
	        var treeState = blackboard.TreeStatus;
	        if (treeState.ContainsKey(Guid))
	        {
	            SavedResult = treeState[Guid];

                if (SavedResult == GoalStatus.Completed || SavedResult == GoalStatus.Failed)
	            {
                    return SavedResult;
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
	        SavedResult = GoalStatus.Inactive;
	    }

	    protected abstract GoalStatus OnUpdate(Blackboard blackboard);
	}
}
