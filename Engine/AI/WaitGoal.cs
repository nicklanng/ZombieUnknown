using System;
using Engine.Entities;

namespace Engine.AI
{
    public class WaitGoal : Goal
    {
        private readonly DrawableEntity _entity;
        private readonly int _millisecondsToWaitFor;
        private int _goalStartedAt;

        public WaitGoal(DrawableEntity entity, int millisecondsToWaitFor)
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

            _entity.SetAnimation("idle", GameState.GameTime);

            var timeGoalRunning = (int)(GameState.GameTime.TotalGameTime.TotalMilliseconds) - _goalStartedAt;
            if (timeGoalRunning >= _millisecondsToWaitFor) 
            {
                GoalStatus = GoalStatus.Completed;
            }
        }
    }
}

