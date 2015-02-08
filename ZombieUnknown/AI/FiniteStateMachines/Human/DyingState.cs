using Engine;
using Engine.AI.FiniteStateMachines;
using Engine.Entities;
using ZombieUnknown.Entities;

namespace ZombieUnknown.AI.FiniteStateMachines.Human
{
    class DyingState : State
    {
        public override string Name { get { return "Dying"; } }

        public override State Update(Entity entity)
        {
            var human = (Entities.Mobiles.Human)entity;
            if (human.IsAnimationComplete())
            {
                GameController.DeleteEntity(human);
                GameController.SpawnEntity(new DeadHuman(human));
            }

            return this;
        }

        public override void OnEnter(Entity entity)
        {
            ((VisibleEntity)entity).SetAnimation("dying");
        }

        public override void OnExit(Entity entity)
        {
        }
    }
}
