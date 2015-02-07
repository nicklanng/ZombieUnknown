﻿using Engine;
using Engine.AI.FiniteStateMachines;
using Engine.Entities;

namespace ZombieUnknown.AI.FiniteStateMachines.Wheat
{
    public class GrowingState : State
    {
        private double _nextStateTrigger;
        
        public override State Update(Entity entity)
        {
            var wheat = (Entities.Wheat)entity;
            wheat.Growth += GameState.GameTime.ElapsedGameTime.TotalMilliseconds;

            if (GameState.GameTime.TotalGameTime.TotalMilliseconds > _nextStateTrigger)
            {
                var nextState = new GrownState();
                nextState.OnEnter(entity);
                return nextState;
            }

            return this;
        }

        public override void OnEnter(Entity entity)
        {
            var wheat = (Entities.Wheat)entity;
            wheat.SetAnimation("growing");

            _nextStateTrigger = GameState.GameTime.TotalGameTime.TotalMilliseconds + 7500 + GameState.RandomNumberGenerator.Next(10000);
        }

        public override void OnExit(Entity entity)
        {
            
        }
    }
}
