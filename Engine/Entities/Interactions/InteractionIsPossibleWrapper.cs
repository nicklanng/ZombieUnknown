using System;

namespace Engine.Entities.Interactions
{
    public class InteractionIsPossibleWrapper<TActor, TSubject>
    {
        public InteractionIsPossibleWrapper(TActor actor, TSubject subject)
        {
            Actor = actor;
            Subject = subject;
        }

        private TActor Actor { get; set; }
        private TSubject Subject { get; set; }

        public bool OfType<TInteraction>() where TInteraction : IInteraction<TSubject, TActor>, new()
        {
            return Activator.CreateInstance<TInteraction>().IsPossible(Subject, Actor);
        }
    }
}