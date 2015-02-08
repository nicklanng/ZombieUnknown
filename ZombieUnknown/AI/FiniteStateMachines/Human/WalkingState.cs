using Engine.AI.FiniteStateMachines;
using Engine.Entities;

namespace ZombieUnknown.AI.FiniteStateMachines.Human
{
    class WalkingState : State
    {
        public override string Name { get { return "Walking"; } }

        public override State Update(Entity entity)
        {
            return this;
        }

        public override void OnEnter(Entity entity)
        {
            ((VisibleEntity)entity).SetAnimation("walk");
        }

        public override void OnExit(Entity entity)
        {
        }
    }
}
