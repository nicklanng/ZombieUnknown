namespace Engine.Entities.Interactions
{
    public class InteractionIsPossibleWrapper<TActor, TSubject> where TSubject : PhysicalEntity where TActor : MobileEntity
    {
        public InteractionIsPossibleWrapper(TActor actor, TSubject subject)
        {
            Actor = actor;
            Subject = subject;
        }

        private TActor Actor { get; set; }
        private TSubject Subject { get; set; }

        public bool OfType<TInteraction>() where TInteraction : InteractionSingleton<TSubject, TActor>, new()
        {
            var targetedInteraction = InteractionManager.GetInteractionOfType<TInteraction>();

            return targetedInteraction.IsPossible(Subject, Actor);
        }
    }
}