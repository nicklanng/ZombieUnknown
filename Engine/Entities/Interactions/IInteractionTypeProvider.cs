using System;

namespace Engine.Entities.Interactions
{
    public interface IInteractionTypeProvider
    {
        Type SubjectType { get; }
        Type ActorType { get; }
    }
}