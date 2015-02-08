using System.Collections.Generic;
using Engine.Entities;

namespace Engine.AI.FiniteStateMachines
{
    public abstract class State
    {
        public abstract string Name { get; }

        protected Dictionary<string, State> Transitions = new Dictionary<string, State>();
        
        public abstract State Update(Entity entity);

        public abstract void OnEnter(Entity entity);

        public abstract void OnExit(Entity entity);

        public virtual State TransitionState(string state, Entity entity)
        {
            if (!Transitions.ContainsKey(state)) return this;

            OnExit(entity);
            var nextState = Transitions[state];
            nextState.OnEnter(entity);
            return nextState;
        }

        public void AddTransition(string transition, State state)
        {
            Transitions.Add(transition, state);
        }
    }
}

