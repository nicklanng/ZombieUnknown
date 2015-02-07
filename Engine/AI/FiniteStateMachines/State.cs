using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Engine.Entities;

namespace Engine.AI.FiniteStateMachines
{
    public abstract class State
    {
        protected Entity Entity;

        public FiniteStateMachine FiniteStateMachine { get; set; }
        public Dictionary<string, State> Transitions { get; set; }

        public State(FiniteStateMachine finiteStateMachine, Entity entity)
        {
            FiniteStateMachine = finiteStateMachine;
            Entity = entity;
            Transitions = new Dictionary<string, State>();
        }

        public void AddTransition(string name, State stateToTransitionTo)
        {
            Transitions.Add(name, stateToTransitionTo);
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void OnEnter(GameTime gameTime) 
        {
        }

        public virtual void OnExit(GameTime gameTime) 
        {
        }
    }
}

