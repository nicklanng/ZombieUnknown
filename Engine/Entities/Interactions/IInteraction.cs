using System;

namespace Engine.Entities.Interactions
{
    public interface IInteraction
    {
        int MillisToCompleteAction { get; }
        Type SubjectType { get; }
        Type ActorType { get; }
    }

    public interface IInteraction<in TSubject, in TActor> : IInteraction
    {
        void Interact(TSubject subject, TActor actor);

        bool IsPossible(TSubject subject, TActor actor);
    }
}