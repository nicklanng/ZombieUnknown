namespace Engine.Entities.Interactions
{
    public interface IInteraction
    {
        int MillisToCompleteAction { get; }
        void Interact(PhysicalEntity entity);
    }
}
