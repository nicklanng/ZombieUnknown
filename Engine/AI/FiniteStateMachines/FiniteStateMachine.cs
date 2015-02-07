using System;
using Microsoft.Xna.Framework;

namespace Engine.AI.FiniteStateMachines
{
    public class FiniteStateMachine
    {
        private State _currentState;

        public FiniteStateMachine ()
        {

        }

        public void Update(GameTime gameTime)
        {
            _currentState.Update(gameTime);
        }

        public void AssignStartingState(State state, GameTime gameTime)
        {
            _currentState = state;
            _currentState.OnEnter(gameTime);
        }

        public void HandleTransition(string transitionName, GameTime gameTime)
        {
            var possibleTransitions = _currentState.Transitions;
            if (!possibleTransitions.ContainsKey(transitionName))
            {
                return;
            }

            var nextState = possibleTransitions[transitionName];

            _currentState.OnExit(gameTime);
            _currentState = nextState;
            _currentState.OnEnter(gameTime);
        }
    }
}

