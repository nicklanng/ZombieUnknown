namespace Engine.Entities.Interactions
{
    public static class InteractionExtensions
    {
        public static InteractionIsPossibleWrapper<TActor, TSubject> CanPerformInteractionOn<TActor, TSubject>(this TActor actor, TSubject subject) where TActor : MobileEntity where TSubject : PhysicalEntity
        {
            return new InteractionIsPossibleWrapper<TActor, TSubject>(actor, subject);
        }
    }
}