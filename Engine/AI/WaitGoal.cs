using Engine.Entities;

namespace Engine.AI
{
    public class WaitGoal : Goal
    {
        private readonly PhysicalEntity _entity;
        private readonly int _millisecondsToWaitFor;
        private int _goalStartedAt;

        public WaitGoal(PhysicalEntity entity, int millisecondsToWaitFor)
        {
            _entity = entity;
            _millisecondsToWaitFor = millisecondsToWaitFor;
        }

        public override void Activate()
        {
            _goalStartedAt = (int)(GameState.GameTime.TotalGameTime.TotalMilliseconds);

            base.Activate();
        }

        public override void Process()
        {
            base.Process();

            if (!IsActive)
            {
                return;
            }

            _entity.SetAnimation("idle");

            var timeGoalRunning = (int)(GameState.GameTime.TotalGameTime.TotalMilliseconds) - _goalStartedAt;
            if (timeGoalRunning >= _millisecondsToWaitFor) 
            {
                GoalStatus = GoalStatus.Completed;
            }
        }
    }
}

