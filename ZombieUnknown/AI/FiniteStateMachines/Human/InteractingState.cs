using Engine.AI.FiniteStateMachines;
using Engine.Entities;

namespace ZombieUnknown.AI.FiniteStateMachines.Human
{
    class InteractingState : State
    {
        public override State Update(Entity entity)
        {
            return this;
        }

        public override void OnEnter(Entity entity)
        {
            ((VisibleEntity)entity).SetAnimation("interact");
        }

        public override void OnExit(Entity entity)
        {
        }
    }
}
