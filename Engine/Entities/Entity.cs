using Engine.AI.FiniteStateMachines;

namespace Engine.Entities
{
    public abstract class Entity
    {
        protected State CurrentState;

        public string Name { get; private set; }

        protected Entity(string name)
        {
            Name = name;
        }

        public virtual void Update() { }

        public void TransitionState(string state)
        {
            if (CurrentState == null) return;

            CurrentState = CurrentState.TransitionState(state, this);
        }
    }
}
