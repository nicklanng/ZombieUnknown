using System;
using Engine.Entities;
using Engine.Maps;
using Engine;
using Engine.AI.FiniteStateMachines;
using Microsoft.Xna.Framework;

namespace ZombieUnknown.Entities
{
    public class Wheat : PhysicalEntity
    {
        private FiniteStateMachine _finiteStateMachine;

        public Wheat(string name, Coordinate mapPosition)
            : base(name, ResourceManager.GetSprite("wheat"), mapPosition)
        {
            _finiteStateMachine = new FiniteStateMachine();
            var sownState = new SownState(_finiteStateMachine, this);
            var growingState = new GrowingState(_finiteStateMachine, this);
            var grownState = new GrownState(_finiteStateMachine, this);

            sownState.AddTransition("growing", growingState);
            growingState.AddTransition("grown", grownState);

            _finiteStateMachine.AssignStartingState(sownState, GameState.GameTime);
        }

        public override void Update(GameTime gameTime)
        {
            _finiteStateMachine.Update(gameTime);

            base.Update(gameTime);
        }
    }

    public class SownState : State
    {
        private double _whenToTransition;

        public SownState(FiniteStateMachine finiteStateMachine, Entity entity) : base(finiteStateMachine, entity) { }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (gameTime.TotalGameTime.TotalMilliseconds > _whenToTransition) 
            {
                FiniteStateMachine.HandleTransition("growing", gameTime);
            }
        }

        public override void OnEnter(GameTime gameTime)
        {
            var wheat = (Wheat)Entity;
            wheat.SetAnimation("sown");

            _whenToTransition = gameTime.TotalGameTime.TotalMilliseconds + 10000;
        }
    }

    public class GrowingState : State
    {
        private double _whenToTransition;

        public GrowingState(FiniteStateMachine finiteStateMachine, Entity entity) : base(finiteStateMachine, entity) { }

        public override void Update (GameTime gameTime)
        {
            base.Update(gameTime);

            if (gameTime.TotalGameTime.TotalMilliseconds > _whenToTransition) 
            {
                FiniteStateMachine.HandleTransition("grown", gameTime);
            }
        }

        public override void OnEnter(GameTime gameTime)
        {
            var wheat = (Wheat)Entity;
            wheat.SetAnimation("growing");

            _whenToTransition = gameTime.TotalGameTime.TotalMilliseconds + 10000;
        }
    }

    public class GrownState : State
    {
        public GrownState(FiniteStateMachine finiteStateMachine, Entity entity) : base(finiteStateMachine, entity) { }

        public override void Update (GameTime gameTime)
        {
            base.Update (gameTime);
        }

        public override void OnEnter(GameTime gameTime)
        {
            var wheat = (Wheat)Entity;
            wheat.SetAnimation("grown");
        }
    }
}

