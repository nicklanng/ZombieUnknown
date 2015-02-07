using Engine.Entities;

namespace Engine.AI.FiniteStateMachines
{
    public abstract class State
    {
        public abstract State Update(Entity entity);

        public abstract void OnEnter(Entity entity);

        public abstract void OnExit(Entity entity);
    }
}

