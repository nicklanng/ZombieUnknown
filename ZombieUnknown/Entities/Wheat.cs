using Engine.Entities;
using Engine.Maps;
using Engine;
using Engine.AI.FiniteStateMachines;
using ZombieUnknown.AI.FiniteStateMachines.Wheat;

namespace ZombieUnknown.Entities
{
    public class Wheat : PhysicalEntity
    {
        private State _currentState;

        public double Growth = 0;

        public Wheat(string name, Coordinate mapPosition)
            : base(name, ResourceManager.GetSprite("wheat"), mapPosition)
        {
            _currentState = WheatStates.Instance.SownState;
            _currentState.OnEnter(this);
        }

        public override void Update()
        {
            _currentState = _currentState.Update(this);

            base.Update();
        }
    }
}

