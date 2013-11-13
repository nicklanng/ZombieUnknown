using Microsoft.Xna.Framework;

namespace Engine.AI
{
    public abstract class Goal : IGoal
    {
        public GoalStatus GoalStatus { get; protected set; }

        public bool IsInactive
        {
            get { return GoalStatus == GoalStatus.Inactive; }
        }

        public bool IsActive
        {
            get { return GoalStatus == GoalStatus.Active; }
        }

        public bool IsComplete
        {
            get { return GoalStatus == GoalStatus.Completed; }
        }

        public bool HasFailed
        {
            get { return GoalStatus == GoalStatus.Failed; }
        }


        public abstract void Activate();

        public virtual void Process(GameTime gameTime)
        {
            ActivateIfInactive();
        }

        public abstract void Terminate();

        private void ActivateIfInactive()
        {
            if (!IsInactive)
            {
                Activate();
            }
        }
    }
}
