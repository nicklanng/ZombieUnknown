using System;
using Engine.AI.FiniteStateMachines;

namespace Engine.Entities
{
    public abstract class Entity
    {
        private bool _isDeleted;

        public event EventHandler Deleted;

        protected State CurrentState;

        public bool IsDeleted
        {
            get { return _isDeleted; }
        }

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

        public void Delete()
        {
            if (_isDeleted) return;

            _isDeleted = true;

            if (Deleted != null)
            {
                Deleted(this, new EventArgs());
            }
        }
    }
}
