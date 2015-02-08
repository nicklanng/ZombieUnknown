using Engine.AI.FiniteStateMachines;
using Engine.Entities;

namespace ZombieUnknown.AI.FiniteStateMachines.Human
{
    class IdleState : State
    {
        public override State Update(Entity entity)
        {
            return this;
        }

        public override void OnEnter(Entity entity)
        {
            ((VisibleEntity)entity).SetAnimation("idle");
        }

        public override void OnExit(Entity entity)
        {
        }
    }
}
