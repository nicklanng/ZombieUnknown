namespace Engine.Entities.Interactions
{
    public interface IInteraction<in TSubject, in TActor> : IInteraction
    {
        void Interact(TSubject subject, TActor actor);

        bool IsPossible(TSubject subject, TActor actor);
    }
}