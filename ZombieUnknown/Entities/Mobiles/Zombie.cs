using Engine;
using Engine.AI.Steering;
using Engine.Entities;
using Engine.Pathfinding;
using Microsoft.Xna.Framework;
using ZombieUnknown.AI;
using ZombieUnknown.AI.FiniteStateMachines.Human;

namespace ZombieUnknown.Entities.Mobiles
{
    class Zombie : MobileEntity, IMovementBlocker
    {
        private readonly ZombieMind _mind;

        public override float MaxVelocity
        {
            get { return 0.1f; }
        }

        public Zombie(string name, Vector2 mapPosition)
            : base(name, ResourceManager.GetSprite("zombie"), mapPosition)
        {
            //_mind = new ZombieMind(this);
            IsStatic = false;

            CurrentState = HumanStates.Instance.IdleState;
            CurrentState.OnEnter(this);

            SeekBehavior = new SeekBehavior(GameState.ZombieTarget);
            AvoidActorsBehavior = new AvoidActorsBehavior();
            ContainmentBehavior = new ContainmentBehavior();
        }

        public override void Update()
        {
            //_mind.Think();

            base.Update();
        }

        public bool BlocksTile { get; private set; }
        public bool BlocksDiagonals { get; private set; }
    }
}
