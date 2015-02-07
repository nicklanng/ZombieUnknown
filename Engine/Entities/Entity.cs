using Engine.AI.FiniteStateMachines;
using Microsoft.Xna.Framework;

namespace Engine.Entities
{
    public abstract class Entity
    {
        private Vector2 _mapPosition;
        protected State CurrentState;

        public string Name { get; private set; }
        public short ZIndex { get; protected set; }
        protected Color LightValue { get; set; }

        public Vector2 MapPosition
        {
            get { return _mapPosition; }
            set
            {
                _mapPosition = value;
            }
        }


        protected Entity(string name, Vector2 mapPosition)
        {
            Name = name;
            MapPosition = mapPosition;

            ZIndex = 0;
        }

        public virtual void Update() { }

        public void TransitionState(string state)
        {
            if (CurrentState == null) return;

            CurrentState = CurrentState.TransitionState(state, this);
        }
    }
}
