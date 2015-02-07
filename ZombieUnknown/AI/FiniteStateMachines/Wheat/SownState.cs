using Engine;
using Engine.AI.FiniteStateMachines;
using Engine.Entities;

namespace ZombieUnknown.AI.FiniteStateMachines.Wheat
{
    public class SownState : State
    {
        private double _nextStateTrigger;

        public SownState()
        {
            Transitions.Add("growing", new GrowingState());
        }
        
        public override State Update(Entity entity)
        {
            var wheat = (Entities.Wheat)entity;
            wheat.Growth += GameState.GameTime.ElapsedGameTime.TotalMilliseconds;

            if (wheat.Growth > _nextStateTrigger)
            {
                return TransitionState("growing", entity);
            }

            return this;
        }

        public override void OnEnter(Entity entity)
        {
            var wheat = (Entities.Wheat)entity;
            wheat.SetAnimation("sown");

            _nextStateTrigger = GameState.GameTime.TotalGameTime.TotalMilliseconds + 7500 + GameState.RandomNumberGenerator.Next(10000);
        }

        public override void OnExit(Entity entity)
        {
        }
    }
}
