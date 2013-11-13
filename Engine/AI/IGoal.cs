using Microsoft.Xna.Framework;

namespace Engine.AI
{
    public interface IGoal
    {
        GoalStatus GoalStatus { get; }

        void Activate();
        void Process(GameTime gameTime);
        void Terminate();
        bool IsComplete { get; }
        bool HasFailed { get; }
    }
}
