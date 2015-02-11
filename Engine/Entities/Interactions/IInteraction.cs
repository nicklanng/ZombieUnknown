using System;

namespace Engine.Entities.Interactions
{
    public interface IInteraction
    {
        int MillisToCompleteAction { get; }
        Type SubjectType { get; }
        Type ActorType { get; }
    }
}