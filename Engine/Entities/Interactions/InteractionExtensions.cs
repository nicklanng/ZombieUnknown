namespace Engine.Entities.Interactions
{
    public static class InteractionExtensions
    {
        public static InteractionIsPossibleWrapper<TActor, TSubject> CanPerformInteractionOn<TActor, TSubject>(this TActor actor, TSubject subject)
        {
            return new InteractionIsPossibleWrapper<TActor, TSubject>(actor, subject);
        }
    }
}