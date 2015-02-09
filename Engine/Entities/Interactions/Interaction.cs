namespace Engine.Entities.Interactions
{
    public abstract class Interaction
    {
        protected readonly PhysicalEntity Subject;

        public virtual int MillisToCompleteAction { get { return 100; } }
        public abstract void Interact(PhysicalEntity actor);

        public abstract bool IsPossible(PhysicalEntity actor);

        protected Interaction(PhysicalEntity subject)
        {
            Subject = subject;
        }
    }
}
