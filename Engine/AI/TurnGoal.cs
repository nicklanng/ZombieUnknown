using Engine.Entities;
using Engine.Maps;
using Microsoft.Xna.Framework;

namespace Engine.AI
{
    public enum TurnDirection 
    {
        Left,
        Right
    }

    public class TurnGoal : Goal
    {
        private readonly DrawableEntity _entity;
        private readonly TurnDirection _turnDirection;

        public TurnGoal(DrawableEntity entity, TurnDirection turnDirection)
        {
            _entity = entity;
            _turnDirection = turnDirection;
        }

        public override void Activate()
        {
            base.Activate();
        }

        public override void Process()
        {
            base.Process();

            if (!IsActive)
            {
                return;
            }

            var newDirection = GetDirectionFromTurnDirection (_turnDirection);

            _entity.FaceDirection(newDirection, GameState.GameTime);

            GoalStatus = GoalStatus.Completed;
        }

        private IDirection GetDirectionFromTurnDirection(TurnDirection turnDirection)
        {
            if (turnDirection == TurnDirection.Left) 
            {
                return _entity.FacingDirection.TurnLeft();
            }
            else 
            {
                return _entity.FacingDirection.TurnRight();
            }
        }
    }
}

