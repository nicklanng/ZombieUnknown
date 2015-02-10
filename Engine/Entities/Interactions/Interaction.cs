namespace Engine.Entities.Interactions
{
    public abstract class Interaction
    {
        public virtual int MillisToCompleteAction
        {
            get { return 100; }
        }

        public abstract void Interact(PhysicalEntity subject, PhysicalEntity actor);

        public abstract bool IsPossible(PhysicalEntity actor);
    }
}