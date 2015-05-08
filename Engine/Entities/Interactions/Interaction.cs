namespace Engine.Entities.Interactions
{
    public abstract class Interaction
    {
        public virtual int MillisToCompleteAction { get { return 100; } }
        public abstract void Interact(MobileEntity actor, PhysicalEntity subject);
    }
}
