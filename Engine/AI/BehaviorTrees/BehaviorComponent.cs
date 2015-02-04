namespace Engine.AI.BehaviorTrees
{
	public abstract class BehaviorComponent
	{
        public GoalStatus GoalStatus { get; protected set; }
        
	    public abstract GoalStatus Update();

	}
}
