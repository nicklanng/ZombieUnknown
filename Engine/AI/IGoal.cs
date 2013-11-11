namespace Engine.AI
{
    public interface IGoal
    {
        GoalStatus GoalStatus { get; }

        void Activate();
        void Process();
        void Terminate();
        bool IsComplete();
        bool HasFailed();
    }
}
