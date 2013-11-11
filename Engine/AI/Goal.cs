namespace Engine.AI
{
    public abstract class Goal : IGoal
    {
        public GoalStatus GoalStatus { get; protected set; }

        public abstract void Activate();

        public abstract void Process();

        public abstract void Terminate();

        public bool IsComplete()
        {
            return GoalStatus == GoalStatus.Completed;
        }

        public bool HasFailed()
        {
            return GoalStatus == GoalStatus.Failed;
        }
    }
}
