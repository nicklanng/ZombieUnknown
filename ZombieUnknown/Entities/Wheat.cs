using System.Collections.Generic;
using Engine.AI.FiniteStateMachines;
using Engine.Entities;
using Engine;
using Engine.Entities.Interactions;
using Microsoft.Xna.Framework;
using ZombieUnknown.AI.FiniteStateMachines.Wheat;

namespace ZombieUnknown.Entities
{
    public class Wheat : VisibleEntity
    {
        public double Growth = 0;

        public Wheat(string name, Vector2 mapPosition)
            : base(name, ResourceManager.GetSprite("wheat"), mapPosition)
        {
            CurrentState = WheatStates.Instance.SownState;
            CurrentState.OnEnter(this);
        }

        public override void Update()
        {
            CurrentState = CurrentState.Update(this);

            base.Update();
        }

        protected override Dictionary<string, Interaction> InteractionList
        {
            get
            {
                var stateActions = (IInteractableState) CurrentState;
                if (stateActions == null)
                {
                    return new Dictionary<string, Interaction>();
                }
                else
                {
                    return stateActions.Interactions;
                }
            }
        }
    }
}

