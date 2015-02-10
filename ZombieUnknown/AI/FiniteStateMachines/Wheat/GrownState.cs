using System;
using System.Collections.Generic;
using Engine.AI.FiniteStateMachines;
using Engine.Entities;
using Engine.Entities.Interactions;
using ZombieUnknown.Entities.Interactions;

namespace ZombieUnknown.AI.FiniteStateMachines.Wheat
{
    public class GrownState : State, IInteractableState
    {
        public override string Name { get { return "Grown"; } }

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

        public Dictionary<string, Interaction> Interactions
        {
            get { return new Dictionary<string, Interaction>
                  
                {
                    { HarvestWheatInteraction.Text, new HarvestWheatInteraction() }
                }; 
            }
        }
    }
}
