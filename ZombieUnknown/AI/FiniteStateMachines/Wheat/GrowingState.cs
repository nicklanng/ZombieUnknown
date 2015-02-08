using Engine;
using Engine.AI.FiniteStateMachines;
using Engine.Entities;

namespace ZombieUnknown.AI.FiniteStateMachines.Wheat
{
    public class GrowingState : State
    {
        private double _nextStateTrigger;

        public override string Name { get { return "Growing"; } }

        public override State Update(Entity entity)
        {
            var wheat = (Entities.Wheat)entity;
            wheat.Growth += GameState.GameTime.ElapsedGameTime.TotalMilliseconds + GameState.RandomNumberGenerator.Next(300) - 150;

            if (wheat.Growth > _nextStateTrigger)
            {
                return TransitionState("grown", entity);
            }

            return this;
        }

        public override void OnEnter(Entity entity)
        {
            var wheat = (Entities.Wheat)entity;
            wheat.SetAnimation("growing");

            _nextStateTrigger = 30000;
        }

        public override void OnExit(Entity entity)
        {
            
        }
    }
}
