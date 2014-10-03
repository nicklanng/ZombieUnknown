using System;
using Engine.Entities;

namespace Engine.AI
{
    public class WaitGoal : Goal
    {
        private int _millisecondsToWaitFor;
        private int _goalStartedAt;

        public WaitGoal(int millisecondsToWaitFor)
        {
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

            var timeGoalRunning = (int)(GameState.GameTime.TotalGameTime.TotalMilliseconds) - _goalStartedAt;
            if (timeGoalRunning >= _millisecondsToWaitFor) 
            {
                GoalStatus = GoalStatus.Completed;
            }
        }
    }
}

