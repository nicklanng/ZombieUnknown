using Engine.Entities;

namespace Engine.AI.FiniteStateMachines
{
    public class FiniteStateMachine
    {
        private State _currentState;

        public void Initialize(Entity entity)
        {
            _currentState.OnEnter(entity);
        }

        public void Update(Entity entity)
        {
            _currentState.Update(entity);
        }

        public void AssignStartingState(State state)
        {
            _currentState = state;
        }

        public void HandleTransition(string transitionName, Entity entity)
        {
            _currentState.OnExit(entity);
            _currentState.OnEnter(entity);
        }
    }
}

