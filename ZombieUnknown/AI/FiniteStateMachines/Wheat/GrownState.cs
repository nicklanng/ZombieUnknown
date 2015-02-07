using Engine.AI.FiniteStateMachines;
using Engine.Entities;

namespace ZombieUnknown.AI.FiniteStateMachines.Wheat
{
    public class GrownState : State
    {
        public override State Update(Entity entity)
        {
            return this;
        }

        public override void OnEnter(Entity entity)
        {
            var wheat = (Entities.Wheat)entity;
            wheat.SetAnimation("grown");
        }

        public override void OnExit(Entity entity)
        {
        }
    }
}
