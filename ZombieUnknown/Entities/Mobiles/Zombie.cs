using Engine;
using Engine.AI.Senses;
using Engine.Entities;
using Engine.Maps;
using ZombieUnknown.AI;
using ZombieUnknown.AI.FiniteStateMachines.Human;

namespace ZombieUnknown.Entities.Mobiles
{
    class Zombie : MobileEntity
    {
        private readonly ZombieMind _mind;

        public override float Speed
        {
            get { return 5; }
        }

        public Vision Vision { get; private set; }

        public Zombie(string name, Coordinate mapPosition)
            : base(name, ResourceManager.GetSprite("zombie"), mapPosition)
        {
            _mind = new ZombieMind(this);
            IsStatic = false;

            CurrentState = HumanStates.Instance.IdleState;
            CurrentState.OnEnter(this);
        }

        public override void Update()
        {
            _mind.Think();

            base.Update();
        }
    }
}
