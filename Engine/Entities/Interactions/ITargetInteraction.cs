namespace Engine.Entities.Interactions
{
    public interface ITargetInteraction
    {
        MobileEntity Actor { get; }
        PhysicalEntity Subject { get; }
        void Interact();
        bool IsPossible();
        int MillisToCompleteAction { get; }
    }
}