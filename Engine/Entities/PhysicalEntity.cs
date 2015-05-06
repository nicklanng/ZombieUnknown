using System.Collections.Generic;
using Engine.AI.FiniteStateMachines;
using Engine.AI.Steering;
using Engine.Entities.Interactions;
using Microsoft.Xna.Framework;

namespace Engine.Entities
{
    public abstract class PhysicalEntity : Entity, ITarget
    {
        protected string CurrentAnimationType;

        protected bool IsStatic = true;

        public Vector2 MapPosition { get; set; }

        protected PhysicalEntity(string name, Vector2 mapPosition)
            : base(name)
        {
            MapPosition = mapPosition;
        }
        
        public virtual AccessPosition[] AccessPositions
        {
            get { return new AccessPosition[0]; }
        }

        protected virtual Dictionary<string, Interaction> InteractionList
        {
            get { return new Dictionary<string, Interaction>(); }
        }

        public Dictionary<string, Interaction> Interactions
        {
            get
            {
                if (CurrentState is IInteractableState)
                {
                    var interactableState = (IInteractableState)CurrentState;
                    return interactableState.Interactions;
                }

                return InteractionList;
            }
        }
    }
}
