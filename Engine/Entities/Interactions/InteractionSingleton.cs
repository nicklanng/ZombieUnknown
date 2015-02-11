using System;

namespace Engine.Entities.Interactions
{
    public abstract class InteractionSingleton<TSubject, TActor> : IInteraction<TSubject, TActor>
    {
        public abstract int MillisToCompleteAction { get; }

        public Type SubjectType
        {
            get { return typeof(TSubject); }
        }

        public Type ActorType
        {
            get { return typeof(TActor); }
        }

        public void Interact(TSubject subject, TActor actor)
        {
            if (IsPossible(subject, actor))
            {
                Execute(subject, actor);
            }
        }

        protected abstract void Execute(TSubject subject, TActor actor);

        public abstract bool IsPossible(TSubject subject, TActor actor);
    }
}